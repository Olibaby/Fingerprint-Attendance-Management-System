using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class DataEntity : DbContext
    {
        public DataEntity() : base("DataEntity")
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentFingerPrint> StudentFingerPrints { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //College
            modelBuilder
                .Entity<College>()
                .HasMany(i => i.Programmes)
                .WithRequired(o => o.College)
                .HasForeignKey(o => o.CollegeId)
                .WillCascadeOnDelete(false);

            //Student
            modelBuilder
                .Entity<Student>()
                .HasOptional(s => s.StudentFingerPrint)
                .WithRequired(sp => sp.Student);
        }
    }
}
