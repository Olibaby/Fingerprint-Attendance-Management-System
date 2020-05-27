using Attendance.Core;
using Attendance.Data;
using DPUruNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class StudentEnrollment : Form
    {
        string errormessage = "";
        int count;
        DataResult<Fmd> resultEnrollment;
        #region Private fields
        List<Fmd> preenrollmentFmds;

        private ReaderCollection _readers;
        private Reader _reader;
        private string _tempFingerPrint;

        DataEntity _db;

        public bool Reset
        {
            get { return reset; }
            set { reset = value; }
        }
        private bool reset;

        public string TempFingerPrint
        {
            get { return _tempFingerPrint; }
            set { _tempFingerPrint = value; }
        }

        private void OneFingerEnrollment_Load(object sender, EventArgs e)
        {
            preenrollmentFmds = new List<Fmd>();
            _db = new DataEntity();
            _readers = ReaderCollection.GetReaders();

            CurrentReader = _readers[0];

            ClearFormFields(this);
            picBoxRightThumb.Image = null;

            if (!OpenReader())
            {
                this.Close();
            }

            if (!StartCaptureAsync(this.OnCaptured))
            {
                this.Close();
            }

            LoadColleges();
            //LoadProgrammes();
            LoadLevels();
            cmdGender.SelectedIndex = 0;
        }

        private void LoadColleges()
        {
            cmbCollege.Items.Clear();
            cmbCollege.SelectedValue = 0;
            cmbCollege.DataSource = null;
            cmbCollege.ValueMember = "CollegeId";
            cmbCollege.DisplayMember = "CollegeName";

            var colleges = new List<College>();
            colleges.Add(new College { CollegeId = 0, CollegeName = "--Select College--" });
            _db.Colleges.ToList().ForEach(i => colleges.Add(new College { CollegeId = i.CollegeId, CollegeName = i.CollegeName }));
            cmbCollege.DataSource = colleges;
        }

        private void LoadProgrammes()
        {
            cmbProgramme.Items.Clear();
            cmbProgramme.DataSource = null;
            cmbProgramme.ValueMember = "ProgrammeId";
            cmbProgramme.DisplayMember = "ProgrammeName";
            cmbProgramme.SelectedValue = 0;

            var programs = new List<Programme>();
            programs.Add(new Programme { ProgrammeId = 0, ProgrammeName = "-- Select Programme--" });
            _db.Programmes.ToList().ForEach(i => programs.Add(new Programme { ProgrammeId = i.ProgrammeId, ProgrammeName = i.ProgrammeName }));
            cmbProgramme.DataSource = programs;
        }

        private void LoadLevels()
        {
            cmbLevel.Items.Clear();
            cmbLevel.DataSource = null;
            cmbLevel.ValueMember = "LevelId";
            cmbLevel.DisplayMember = "LevelName";
            cmbLevel.Items.Insert(0, "-- Select Level--");
            cmbLevel.SelectedValue = 0;

            var levels = new List<Level>();
            levels.Add(new Level { LevelId = 0, LevelName = "--Select Level--" });
            _db.Levels.ToList().ForEach(i => levels.Add(new Level { LevelId = i.LevelId, LevelName = i.LevelName }));
            cmbLevel.DataSource = levels;
        }
        private StudentModel Saved { get; set; }
        #endregion

        public delegate void CaptureCallback(CaptureResult result);
        public Reader CurrentReader
        {
            get { return _reader; }
            set { _reader = value; }
        }
        public StudentEnrollment()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var filled = FillStudentData();

            if (CanSave())
            {
                SaveStudent(filled);
                MessageBox.Show("Student is Successfully Registered");
                ClearFormFields(this);
                return;
            }
            MessageBox.Show("" + errormessage);
        }

        private Student FillStudentData()
        {

            var std = new Student();

            std.MatricNo = txtMatricNo.Text;
            std.CollegeId = int.Parse(cmbCollege.SelectedValue.ToString());
            std.ProgrammeId = int.Parse(cmbProgramme.SelectedValue.ToString());
            std.LevelId = int.Parse(cmbLevel.SelectedValue.ToString());
            std.LastName = txtLastName.Text;
            std.FirstName = txtFirstName.Text;
            std.MiddleName = txtMiddleName.Text;
            std.Email = txtEmail.Text;
            std.Phone = txtPhoneNo.Text;

            std.Gender = cmdGender.SelectedItem.ToString();
            std.CreatedBy = "Admin";
            std.CreatedDate = DateTime.Now;
            std.FingerPrint = TempFingerPrint;

            /*return new Student
            {
                MatricNo = txtMiddleName.Text,
                CollegeId = int.Parse(cmbCollege.SelectedValue.ToString()),
                ProgramId = int.Parse(cmbProgram.SelectedValue.ToString()),
                LevelId = int.Parse(cmbLevel.SelectedValue.ToString()),
                LastName = txtLastName.Text,
                FirstName = txtFirstName.Text,
                MiddleName = txtMiddleName.Text,
                Email = txtEmail.Text,
                Phone = txtPhoneNo.Text,

                Gender = sex,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now
            };*/

            return std;
        }
        private void SaveStudent(Student model)
        {
            //Chekck if student already exists
            var student = _db.Students.Where(s => s.MatricNo == model.MatricNo).FirstOrDefault();
            if(student != null)
            {
                MessageBox.Show("This student has already registered");
            }

            var user = new User {UserName = model.Email, Password = model.MatricNo, CreatedBy = model.FirstName, CreatedDate = DateTime.Now };
            _db.Users.Add(user);
            var userId = _db.SaveChanges();
            model.UserId = userId;
            _db.Students.Add(model);
            _db.SaveChanges();
            preenrollmentFmds.Clear();
            picBoxRightThumb.Image = null;
            count = 0;
            lblCount.Invoke(new Action(() => { lblCount.Text = count.ToString(); }));
        }
        public bool CanSave()
        {
            bool cansave = true;

            if (txtMatricNo.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "MatricNo is required.\n";
            }
            if (txtLastName.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "Surname is required.\n";
            }
            if (txtFirstName.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "First Names are required.\n";
            }
            if (txtMiddleName.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "Middle Names are required.\n";
            }
            if (txtPhoneNo.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "Phone No are required.\n";
            }
            if (cmbCollege.SelectedIndex == 0 && cmbCollege.Visible)
            {
                cansave = false;
                errormessage += "College is required.\n";
            }
            if (cmbProgramme.SelectedIndex == 0 && cmbProgramme.Visible)
            {
                cansave = false;
                errormessage += "Program is required.\n";
            }
            if (cmbLevel.SelectedIndex == 0 && cmbLevel.Visible)
            {
                cansave = false;
                errormessage += "Level is required.\n";
            }
            if (picBoxRightThumb.Visible && picBoxRightThumb.Image == null)
            {
                cansave = false;
                errormessage += "Left Thumb fingerprint is required.\n";
            }
            if (resultEnrollment.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                cansave = false;
                errormessage += "Make sure you scan same finger at least four times.\n";
            }
            return cansave;
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
                        c.BeginInvoke(
                                  new Action(() =>
                                  {
                                      ((TextBox)c).Clear();
                                  }
                        ));
                    }
                    else
                    {
                        ((TextBox)c).Clear();
                    }

                }
            }
        }

        public void GetStatus()
        {
            Constants.ResultCode result = _reader.GetStatus();

            if ((result != Constants.ResultCode.DP_SUCCESS))
            {
                if (CurrentReader != null)
                {
                    CurrentReader.Dispose();
                    CurrentReader = null;
                }
                throw new Exception("" + result);
            }

            if ((_reader.Status.Status == Constants.ReaderStatuses.DP_STATUS_BUSY))
            {
                Thread.Sleep(50);
            }
            else if ((_reader.Status.Status == Constants.ReaderStatuses.DP_STATUS_NEED_CALIBRATION))
            {
                _reader.Calibrate();
            }
            else if ((_reader.Status.Status != Constants.ReaderStatuses.DP_STATUS_READY))
            {
                throw new Exception("Reader Status - " + _reader.Status.Status);
            }
        }
        public bool CaptureFingerAsync()
        {
            try
            {
                GetStatus();

                Constants.ResultCode captureResult = _reader.CaptureAsync(Constants.Formats.Fid.ANSI, Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, _reader.Capabilities.Resolutions[0]);
                if (captureResult != Constants.ResultCode.DP_SUCCESS)
                {
                    reset = true;
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
            if (_reader == null) return true;
            // Activate capture handler
            _reader.On_Captured += new Reader.CaptureCallback(OnCaptured);

            // Call capture
            if (!CaptureFingerAsync())
            {
                return false;
            }

            return true;
        }

        public bool OpenReader()
        {
            reset = false;
            Constants.ResultCode result = Constants.ResultCode.DP_DEVICE_FAILURE;
            if (CurrentReader == null) return true;

            // Open reader
            result = CurrentReader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);

            if (result != Constants.ResultCode.DP_SUCCESS)
            {
                MessageBox.Show("Error:  " + result);
                reset = true;
                return false;
            }

            return true;
        }

        int fingerToCapture;
        PictureBox picBoxToCap;
        public void OnCaptured(CaptureResult captureResult)
        {
            this.picBoxToCap = picBoxRightThumb;
            this.fingerToCapture = 0;
            count++;
            lblCount.Invoke(new Action(() => { lblCount.Text = count.ToString(); }));
            DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);

            if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                if (CurrentReader != null)
                {
                    CurrentReader.Dispose();
                    CurrentReader = null;
                }

                MessageBox.Show("Error:  " + captureResult.ResultCode);
            }
            if (captureResult.Data != null)
            {
                foreach (Fid.Fiv fiv in captureResult.Data.Views)
                {
                    picBoxRightThumb.Image = CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height);
                    picBoxRightThumb.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            preenrollmentFmds.Add(resultConversion.Data);
            if (count >= 4)
            {
                resultEnrollment = Enrollment.CreateEnrollmentFmd(Constants.Formats.Fmd.ANSI, preenrollmentFmds);

                TempFingerPrint = Fmd.SerializeXml(resultConversion.Data);

                resultEnrollment = DPUruNet.Enrollment.CreateEnrollmentFmd(Constants.Formats.Fmd.ANSI, preenrollmentFmds);

                if (resultEnrollment.ResultCode == Constants.ResultCode.DP_SUCCESS)
                {
                    FileStream fs = new System.IO.FileStream(@"..\..\Pictures\right.bmp", FileMode.Open, FileAccess.Read);
                    picRigithWrong.Invoke(new Action(() => { picRigithWrong.Image = Image.FromStream(fs); }));
                    picRigithWrong.SizeMode = PictureBoxSizeMode.Zoom;
                    //preenrollmentFmds.Clear();
                    //count = 0;
                    MessageBox.Show("An enrollment was successfully created.");
                    return;
                }
                else if (resultEnrollment.ResultCode == Constants.ResultCode.DP_ENROLLMENT_INVALID_SET)
                {
                    MessageBox.Show("Enrollment was unsuccessful.  Please try again.");
                    preenrollmentFmds.Clear();
                    count = 0;
                    return;
                }
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

        private void OneFingerEnrollment_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentReader != null)
            {
                // Dispose of reader handle and unhook reader events.
                CurrentReader.Dispose();

                if (reset)
                {
                    CurrentReader = null;
                }
            }
        }

        private void cmbCollege_SelectedIndexChanged(object sender, EventArgs e)
        {
            var collegeId = int.Parse(this.cmbCollege.SelectedValue.ToString());
            cmbProgramme.DataSource = null;
            cmbProgramme.ValueMember = "ProgrammeId";
            cmbProgramme.DisplayMember = "ProgrammeName";

            var programs = new List<Programme>();
            programs.Add(new Programme { ProgrammeId = 0, ProgrammeName = "-- Select Programme--" });
            _db.Programmes.Where(p => p.CollegeId == collegeId).ToList().ForEach(i => programs.Add(new Programme { ProgrammeId = i.ProgrammeId, ProgrammeName = i.ProgrammeName }));
            cmbProgramme.DataSource = programs;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
