using Attendance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class CollegeModel
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeDean { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ICollection<LecturerModel> Lecturers { get; set; }
        public ICollection<StudentModel> Students { get; set; }
        public ICollection<ProgrammeModel> Programs { get; set; }
        public CollegeModel()
        {
            Students = new HashSet<StudentModel>();
            Lecturers = new HashSet<LecturerModel>();
            Programs = new HashSet<ProgrammeModel>();
        }

        public CollegeModel(College college)
        {
            if (college == null) return;
            CollegeId = college.CollegeId;
            CollegeName = college.CollegeName;
            CollegeDean = college.CollegeDean;

            Lecturers = new HashSet<LecturerModel>();
            Students = new HashSet<StudentModel>();
        }

        public College Create(CollegeModel model)
        {
            return new College
            {
                CollegeName = model.CollegeName,
                CollegeDean = model.CollegeDean,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };
        }

        public College Edit(College entity, CollegeModel model)
        {
            entity.CollegeId = model.CollegeId;
            entity.CollegeName = model.CollegeName;
            entity.CollegeDean = model.CollegeDean;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = model.ModifiedDate;
            return entity;
        }
    }
}
