using Attendance.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.windows
{
    public partial class Courses : Form
    {
        public Courses()
        {
            InitializeComponent();
        }

        private void Courses_Load(object sender, EventArgs e)
        {
            var _db = new DataEntity();
            comboBox1.DataSource = _db.Courses.ToList();
            comboBox1.DisplayMember = "CourseName";
        }
    }
}
