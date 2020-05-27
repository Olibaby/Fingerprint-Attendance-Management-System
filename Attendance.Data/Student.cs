using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MatricNo { get; set; }
        public int? ProgrammeId { get; set; }
        public int? CollegeId { get; set; }
        public int? LevelId { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string FingerPrint { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Programme Programme { get; set; }
        public College College { get; set; }
        public Level Level { get; set; }
        public StudentFingerPrint StudentFingerPrint { get; set; }

        public Student()
        {
           //new Programme();
           //new College();
           //new Level();
           //new StudentFingerPrint();
        }
    }
}
