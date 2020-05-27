using Attendance.Data;
using DPUruNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.windows
{
    public partial class Form_Main : Form
    {
        string errormessage = "";
        DataEntity _db;
        private const int DPFJ_PROBABILITY_ONE = 0x7fffffff;
        private Fmd currentFinger;

        public bool Reset { get; set; }

        private ReaderCollection _readers;
        public Reader CurrentReader { get; set; }
        public Form_Main()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Check quality of the resulting capture.
        /// </summary>
        public bool CheckCaptureResult(CaptureResult captureResult)
        {
            if (captureResult.Data == null)
            {
                if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    Reset = true;
                    throw new Exception(captureResult.ResultCode.ToString());
                }

                // Send message if quality shows fake finger
                if ((captureResult.Quality != Constants.CaptureQuality.DP_QUALITY_CANCELED))
                {
                    throw new Exception("Quality - " + captureResult.Quality);
                }
                return false;
            }

            return true;
        }

        public bool CaptureFingerAsync()
        {
            try
            {
                GetStatus();

                Constants.ResultCode captureResult = CurrentReader.CaptureAsync(Constants.Formats.Fid.ANSI, Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, CurrentReader.Capabilities.Resolutions[0]);
                if (captureResult != Constants.ResultCode.DP_SUCCESS)
                {
                    Reset = true;
                    throw new Exception("" + captureResult);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:  " + ex.Message);
                return false;
            }
        }

        public bool StartCaptureAsync(Reader.CaptureCallback OnCaptured)
        {
            if (CurrentReader == null) return true;
            // Activate capture handler
            CurrentReader.On_Captured += new Reader.CaptureCallback(OnCaptured);

            // Call capture
            if (!CaptureFingerAsync())
            {
                return false;
            }

            return true;
        }
        public bool OpenReader()
        {
            Reset = false;
            Constants.ResultCode result = Constants.ResultCode.DP_DEVICE_FAILURE;

            if(CurrentReader == null) return true;
            // Open reader
            result = CurrentReader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);

            if (result != Constants.ResultCode.DP_SUCCESS)
            {
                MessageBox.Show("Error:  " + result);
                Reset = true;
                return false;
            }

            return true;
        }
        public void GetStatus()
        {
            Constants.ResultCode result = CurrentReader.GetStatus();

            if ((result != Constants.ResultCode.DP_SUCCESS))
            {
                if (CurrentReader != null)
                {
                    CurrentReader.Dispose();
                    CurrentReader = null;
                }
                throw new Exception("" + result);
            }

            if ((CurrentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_BUSY))
            {
                Thread.Sleep(50);
            }
            else if ((CurrentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_NEED_CALIBRATION))
            {
                CurrentReader.Calibrate();
            }
            else if ((CurrentReader.Status.Status != Constants.ReaderStatuses.DP_STATUS_READY))
            {
                throw new Exception("Reader Status - " + CurrentReader.Status.Status);
            }
        }

        public Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }

            bmp.UnlockBits(data);

            return bmp;
        }

        private void Verification_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentReader != null)
            {
                // Dispose of reader handle and unhook reader events.
                CurrentReader.Dispose();

                if (Reset)
                {
                    CurrentReader = null;
                }
            }
        }

        private void ClearFormFields(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c.HasChildren)
                {
                    ClearFormFields(c);
                }
                else if (c is TextBox)
                {
                    if (c.InvokeRequired)
                    {
                        c.BeginInvoke(new Action(() => {((TextBox)c).Clear(); }));
                    }
                    else
                    {
                        ((TextBox)c).Clear();
                    }

                }
            }
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {

        }

        private void enrollStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var one = new StudentEnrollment();
            one.ShowDialog();
        }

        private void takeAttendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indentify = new Verification();
            indentify.ShowDialog();
            indentify.Dispose();
            indentify = null;
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Logout();
            }
        }

        public void Logout()
        {
                Dispose();
        }

        private void editStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indentify = new EditStudentEnrollment();
            indentify.ShowDialog();
        }
    }
}
