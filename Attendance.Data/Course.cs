using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CollegeId { get; set; }
        public int ProgrammeId { get; set; }
        public int LevelId { get; set; }
        public string Semester { get; set; }
        public int? LecturerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public College College { get; set; }
        public Level Level { get; set; }
        public Lecturer Lecturer { get; set; }
        public Programme Programme { get; set; }

       /* public Course()
        {
            Level = new Level();
            College = new College();
            Lecturer = new Lecturer();
            Programme = new Programme();
        }*/
    }
}
