using Attendance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class LevelModel
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<StudentModel> Students { get; set; }

        public LevelModel()
        {
           new HashSet<StudentModel>();
        }

        public LevelModel(Level level)
        {
            if (level == null) return;
            LevelId = level.LevelId;
            LevelName = level.LevelName;
            Students = new HashSet<StudentModel>();
        }

        public Level Create(LevelModel model)
        {
            return new Level
            {
                LevelName = model.LevelName,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
            };
        }

        public Level Edit(Level entity, LevelModel model)
        {
            entity.LevelId = model.LevelId;
            entity.LevelName = model.LevelName;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = model.ModifiedDate;
            return entity;
        }
    }
}
