using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class College
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeDean { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ICollection<Lecturer> Lecturers { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Programme> Programmes { get; set; }

        public College()
        {
            Students = new HashSet<Student>();
            Lecturers = new HashSet<Lecturer>();
            Programmes = new HashSet<Programme>();
        }
    }
}
