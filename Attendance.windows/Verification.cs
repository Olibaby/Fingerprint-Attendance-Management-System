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
using CurrentAttendance = Attendance.Data.Attendance;

namespace Attendance.windows
{
    public partial class Verification : Form
    {
        string errormessage = "";
        DataEntity _db;
        private const int DPFJ_PROBABILITY_ONE = 0x7fffffff;
        private Fmd currentFinger;

        public bool Reset { get; set; }

        private ReaderCollection _readers;
        public Reader CurrentReader { get; set; }
        public Verification()
        {
            InitializeComponent();
        }

        private void OnCaptured(CaptureResult captureResult)
        {
            try
            {
                // Check capture quality and throw an error if bad.
                if (!CheckCaptureResult(captureResult)) return;

                //SendMessage(Action.SendMessage, "A finger was captured.");

                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    Reset = true;
                    throw new Exception(captureResult.ResultCode.ToString());
                }


                currentFinger = resultConversion.Data;
                var fmds = new List<Fmd>();
                var students = _db.Students.ToList();

                //get string version of the current fmd
                //Fmd fmd = Fmd.DeserializeXml(student.FingerPrint);

                foreach (var student in students)
                {
                    if (student.FingerPrint == null) continue;
                    Fmd fmd = Fmd.DeserializeXml(student.FingerPrint);
                    CompareResult compareResult = Comparison.Compare(currentFinger, 0, fmd, 0);
                    if (compareResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        Reset = true;
                        throw new Exception(compareResult.ResultCode.ToString());
                    }
                    else
                    {
                        if (compareResult.Score < (DPFJ_PROBABILITY_ONE / 100000))
                        {

                            if (captureResult.Data != null)
                            {
                                foreach (Fid.Fiv fiv in captureResult.Data.Views)
                                {
                                    pictureBox.Image = CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height);
                                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                            }

                            FileStream fs = new System.IO.FileStream(@"..\..\Pictures\right.bmp", FileMode.Open, FileAccess.Read);

                            lblrstLastName.Invoke(new Action(() => { lblrstLastName.Text = student.LastName; }));
                            lblrstFirstName.Invoke(new Action(() => { lblrstFirstName.Text = student.FirstName; }));
                            lblrstMiddleName.Invoke(new Action(() => { lblrstMiddleName.Text = student.MiddleName; }));
                            lblrstMatricNo.Invoke(new Action(() => { lblrstMatricNo.Text = student.MatricNo; }));
                            lblrstEmail.Invoke(new Action(() => { lblrstEmail.Text = student.Email; }));
                            lblrstPhone.Invoke(new Action(() => { lblrstPhone.Text = student.Phone; }));
                            cmbCollege.Invoke(new Action(() => { cmbCollege.SelectedValue = student.CollegeId; }));
                            cmbProgramme.Invoke(new Action(() => { cmbProgramme.SelectedValue = student.ProgrammeId; }));
                            cmbLevel.Invoke(new Action(() => { cmbLevel.SelectedValue = student.LevelId; }));
                            //pictureBox.Invoke(new Action(() => { pictureBox1.Image = Image.FromFile(@"..\..\Pictures\right.bmp"); }));
                            pictureBox1.Invoke(new Action(() => { pictureBox1.Image = Image.FromStream(fs); }));
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            fs.Close();
                            return;
                        }
                        else
                        {
                            //MessageBox.Show("The Student does not exist in the database");
                        }
                    }

                };

                //SendMessage(Action.SendMessage, "Identification resulted in the following number of matches: " + identifyResult.Indexes.Length.ToString());
                //SendMessage(Action.SendMessage, "Place your right index finger on the reader.");
                //}
            }
            catch (Exception ex)
            {
                // Send error message, then close form
                //SendMessage(Action.SendMessage, "Error:  " + ex.Message);
            }
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

        private void Verification_Load(object sender, EventArgs e)
        {
            _db = new DataEntity();
            _readers = ReaderCollection.GetReaders();
            CurrentReader = _readers[0];
            if (!OpenReader())
            {
                this.Close();
            }

            if (!StartCaptureAsync(this.OnCaptured))
            {
                this.Close();
            }
            LoadColleges();
            LoadLevels();
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
        private void cmbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            var collegeId = int.Parse(this.cmbCollege.SelectedValue.ToString());
            var programmeId = int.Parse(this.cmbProgramme.SelectedValue.ToString());
            var levelId = int.Parse(this.cmbLevel.SelectedValue.ToString());
            var semester = this.cmbSemester.SelectedItem.ToString();
            cmbCourse.DataSource = null;
            cmbCourse.ValueMember = "CourseId";
            cmbCourse.DisplayMember = "CourseName";

            var courses = new List<Course>();
            courses.Add(new Course { CourseId = 0, CourseName = "-- Select Course--" });
            _db.Courses.Where(p => p.CollegeId == collegeId && p.ProgrammeId == programmeId && p.LevelId == levelId && p.Semester == semester)
                .ToList().ForEach(i => courses.Add(new Course { CourseId = i.CourseId, CourseName = i.CourseCode + " " + i.CourseName }));
            cmbCourse.DataSource = courses;
        }

        private void bntAddAttendance_Click(object sender, EventArgs e)
        {
            var attendance = FillAttendanceWithData();

            if (CanSave())
            {
                SaveAttendance(attendance);
                MessageBox.Show("Attendance is Successfully Marked");
                ClearFormFields(this);
                return;
            }
            MessageBox.Show("" + errormessage);
        }

        private void SaveAttendance(CurrentAttendance model)
        {
            var student = _db.Attendances.Where(c => c.MatricNo == model.MatricNo && c.CourseId == model.CourseId && DbFunctions.TruncateTime(c.AttendanceDate) == model.AttendanceDate.Date).AsNoTracking().FirstOrDefault();
            if (student != null)
            {
                MessageBox.Show("This student Attendance already taken for this course on this date");
            }
            else
            { 
            _db.Attendances.Add(model);
            _db.SaveChanges();
            }
        }

        private CurrentAttendance FillAttendanceWithData()
        {
            var attendance = new CurrentAttendance();

            attendance.MatricNo = lblrstMatricNo.Text;
            attendance.LastName = lblrstLastName.Text;
            attendance.FirstName = lblrstFirstName.Text;
            attendance.MiddleName = lblrstMiddleName.Text;
            attendance.CollegeId = int.Parse(cmbCollege.SelectedValue.ToString());
            attendance.ProgrammeId = int.Parse(cmbProgramme.SelectedValue.ToString());
            attendance.CourseId = int.Parse(cmbCourse.SelectedValue.ToString());
            attendance.Semester = cmbSemester.SelectedItem.ToString();
            attendance.StudentId = _db.Students.Where(s => s.MatricNo == attendance.MatricNo).Select(s => s.StudentId).FirstOrDefault();

            attendance.AttendanceDate = dateTimePicker1.Value;
            attendance.CreatedBy = lblrstMatricNo.Text;
            attendance.CreatedDate = DateTime.Now;
            return attendance;
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

        public bool CanSave()
        {
            bool cansave = true;

            if (lblMatricNo.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "MatricNo is required.\n";
            }
            if (lblLastName.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "Surname is required.\n";
            }
            if (lblFirstName.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "First Names are required.\n";
            }
            if (lblMiddleName.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "Middle Names are required.\n";
            }
            if (lblPhoneNo.Text.Trim().Length == 0)
            {
                cansave = false;
                errormessage += "Other Names are required.\n";
            }
            if (cmbCollege.SelectedIndex == 0)
            {
                cansave = false;
                errormessage += "College is required.\n";
            }
            if (cmbProgramme.SelectedIndex == 0)
            {
                cansave = false;
                errormessage += "Program is required.\n";
            }
            if (cmbLevel.SelectedIndex == 0)
            {
                cansave = false;
                errormessage += "Level is required.\n";
            }
            if (cmbSemester.SelectedIndex == -1)
            {
                cansave = false;
                errormessage += "Semeter is required.\n";
            }
            if (cmbCourse.SelectedIndex == -1)
            {
                cansave = false;
                errormessage += "Course is required.\n";
            }
            return cansave;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
