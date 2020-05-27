using Attendance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class LecturerModel 
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string StaffNo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? ProgrammeId { get; set; }
        public int? CollegeId { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ProgrammeModel Programme { get; set; }
        public CollegeModel College { get; set; }
        public ICollection<CourseModel> Courses { get; set; }
        public LecturerModel()
        {
            Courses = new HashSet<CourseModel>();
            Programme = new ProgrammeModel();
            College = new CollegeModel();
        }

        public LecturerModel(Lecturer lecturer)
        {
            if (lecturer == null) return;
            LecturerId = lecturer.LecturerId;
            CollegeId = lecturer.CollegeId;
            ProgrammeId = lecturer.ProgrammeId;
            FirstName = lecturer.FirstName;
            MiddleName = lecturer.MiddleName;
            LastName = lecturer.LastName;
            Title = lecturer.Title;
            StaffNo = lecturer.StaffNo;
            Email = lecturer.Email;
            Gender = lecturer.Gender;

            Programme = new ProgrammeModel();
            College = new CollegeModel();
            Courses = new HashSet<CourseModel>();
        }

        public Lecturer Create(LecturerModel model)
        {
            return new Lecturer
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Title = model.Title,
                StaffNo = model.StaffNo,
                Email = model.Email,
                Gender = model.Gender,
                ProgrammeId = model.ProgrammeId,
                CollegeId = model.CollegeId,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };
        }

        public Lecturer Edit(Lecturer entity, LecturerModel model)
        {
            entity.FirstName = model.FirstName;
            entity.MiddleName = model.MiddleName;
            entity.LastName = model.LastName;
            entity.Title = model.Title;
            entity.StaffNo = model.StaffNo;
            entity.Email = model.Email;
            entity.Gender = model.Gender;
            entity.ProgrammeId = model.ProgrammeId;
            entity.CollegeId = model.CollegeId;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = model.ModifiedDate;
            return entity;
        }
    }
}
