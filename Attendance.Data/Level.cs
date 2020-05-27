using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class Level
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<Student> Students { get; set; }

        public Level()
        {
            this.Students = new HashSet<Student>();
        }
    }
}
