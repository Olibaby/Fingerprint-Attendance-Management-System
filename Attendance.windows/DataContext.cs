using Attendance.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.windows
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataEntity")
        {
             
        }

        public DbSet<Student> Students { get; set; }
    }
}
