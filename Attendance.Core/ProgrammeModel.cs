using Attendance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class ProgrammeModel
    {

        public int ProgrammeId { get; set; }
        public string ProgrammeName { get; set; }
        public int CollegeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public College College { get; set; }
        public ICollection<LecturerModel> Lecturers { get; set; }
        public ICollection<StudentModel> Students { get; set; }
        public ICollection<CourseModel> Courses { get; set; }
        public ProgrammeModel()
        {
            Students = new HashSet<StudentModel>();
            Lecturers = new HashSet<LecturerModel>();
            Courses = new HashSet<CourseModel>();
        }

        public ProgrammeModel(Programme programme)
        {
            if (programme == null) return;
            ProgrammeId = programme.ProgrammeId;
            ProgrammeName = programme.ProgrammeName;
            Lecturers = new HashSet<LecturerModel>();
            Students = new HashSet<StudentModel>();
        }

        public Programme Create(ProgrammeModel model)
        {
            return new Programme
            {
                CollegeId = model.CollegeId,
                ProgrammeName = model.ProgrammeName,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
            };
        }

        public Programme Edit(Programme entity, ProgrammeModel model)
        {
            entity.CollegeId = model.CollegeId;
            entity.ProgrammeName = model.ProgrammeName;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = model.ModifiedDate;
            return entity;
        }
    }
}
