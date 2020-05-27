using Attendance.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode{ get; set; }
        [Required]
        public int CollegeId { get; set; }
        public int ProgrammeId { get; set; }
        public int LevelId { get; set; }
        public string Semester { get; set; }
        public int? LecturerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public  LecturerModel Lecturer { get; set; }
        public CollegeModel College { get; set; }
        public LevelModel Level { get; set; }
        public ProgrammeModel Programme { get; set; }
        public CourseModel()
        {
            new LecturerModel();
        }

        public CourseModel(Course course)
        {
            if (course == null) return;
            CourseId = course.CourseId;
            CourseName = course.CourseName;
            CourseCode = course.CourseCode;
            CollegeId = course.CollegeId;
            ProgrammeId = course.ProgrammeId;
            LevelId = course.LevelId;
            LecturerId = course.LecturerId;
            Lecturer = new LecturerModel();
        }

        public Course Create(CourseModel model)
        {
            return new Course
            {
                CourseName = model.CourseName,
                CourseCode = model.CourseCode,
                CollegeId = model.CollegeId,
                ProgrammeId = model.ProgrammeId,
                LevelId = model.LevelId,
                LecturerId = model.LecturerId,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
            };
        }

        public Course Edit(Course entity, CourseModel model)
        {
            entity.CourseId = model.CourseId;
            entity.CourseName = model.CourseName;
            entity.CourseCode = model.CourseCode;
            entity.CollegeId = model.CollegeId;
            entity.ProgrammeId = model.ProgrammeId;
            entity.LevelId = model.LevelId;
            entity.LecturerId = model.LecturerId;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = model.ModifiedDate;
            return entity;
        }
    }
}
