using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class Programme
    {
        public int ProgrammeId { get; set; }
        public string ProgrammeName { get; set; }
        public int CollegeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public College College { get; set; }
        public  ICollection<Lecturer> Lecturers { get; set; }
        public  ICollection<Student> Students { get; set; }
        public ICollection<Course> Courses { get; set; }

        public Programme()
        {
            //new College();
            Students = new HashSet<Student>();
            Lecturers = new HashSet<Lecturer>();
            Courses = new HashSet<Course>();
        }
    }
}
