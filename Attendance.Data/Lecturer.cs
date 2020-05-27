using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string StaffNo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int UserId { get; set; }
        public int? ProgrammeId { get; set; }
        public int? CollegeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; } 
        public DateTime? ModifiedDate { get; set; }

        public  Programme Programme { get; set; }
        public  College College { get; set; }
        public  ICollection<Course> Courses { get; set; }
        public Lecturer()
        {
            this.Courses = new HashSet<Course>();
            //new Programme();
            //new College();
        }
    }
}
