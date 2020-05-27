using Attendance.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MatricNo { get; set; }
        [Required]
        public int? ProgrammeId { get; set; }
        [Required]
        public int? CollegeId { get; set; }
        [Required]
        public int? LevelId { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ProgrammeModel Programme { get; set; }
        public CollegeModel College { get; set; }
        public LevelModel Level { get; set; }

        public StudentModel()
        {
            new ProgrammeModel();
            new CollegeModel();
            new LevelModel();
        }

        public StudentModel(Student student)
        {
            if (student == null) return;
            StudentId = student.StudentId;
            FirstName = student.FirstName;
            MiddleName = student.MiddleName;
            LastName = student.LastName;
            MatricNo = student.MatricNo;
            Email = student.Email;
            Phone = student.Phone;
            Gender = student.Gender;
            ProgrammeId = student.ProgrammeId;
            CollegeId = student.CollegeId;
            LevelId = student.LevelId;

            Programme = new ProgrammeModel();
            College = new CollegeModel();
            Level = new LevelModel();
        }

        public Student Create(StudentModel model)
        {
            return new Student
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                MatricNo = model.MatricNo,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                ProgrammeId = model.ProgrammeId,
                CollegeId = model.CollegeId,
                LevelId = model.LevelId,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
            };
        }

        public Student Edit(Student entity, StudentModel model)
        {
            entity.StudentId = model.StudentId;
            entity.FirstName = model.FirstName;
            entity.MiddleName = model.MiddleName;
            entity.LastName = model.LastName;
            entity.MatricNo = model.MatricNo;
            entity.Email = model.Email;
            entity.Phone = model.Phone;
            entity.Gender = model.Gender;
            entity.ProgrammeId = model.ProgrammeId;
            entity.CollegeId = model.CollegeId;
            entity.LevelId = model.LevelId;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = model.ModifiedDate;
            return entity;
        }
    }
}

