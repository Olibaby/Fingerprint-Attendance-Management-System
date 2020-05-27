using Attendance.Core.Interface;
using Attendance.Data;
using Attendance.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Manager
{
    public class AttendanceManager : IAttendanceManager
    {
        private IGenericRepository _repo;
        public AttendanceManager(IGenericRepository repo)
        {
            _repo = repo;
        }
        #region college
        public void Add(CollegeModel model)
        {
            var entity = model.Create(model);
            _repo.Add<College>(entity);
        }

        public CollegeModel GetCollege(int? id)
        {
            var model = _repo.Get<College>(id);
            return new CollegeModel(model);
        }

        public CollegeModel[] GetColleges()
        {
            var entities = _repo.Get<College>();
            var models = entities.Select(s => new CollegeModel(s)).ToArray();
            return models;
        }

        public void RemoveCollege(int id)
        {
            var entity = _repo.Get<College>(id);
            _repo.Remove<College>(id);
        }

        public void Update(int id, CollegeModel model)
        {
            var college = _repo.Get<College>(model.CollegeId);
            var entity = model.Edit(college, model);
            _repo.Update<College>(model.CollegeId, entity);
        }
        #endregion

        #region Course
        public void Add(CourseModel model)
        {
            var entity = model.Create(model);
            _repo.Add<Course>(entity);
        }

        public CourseModel GetCourse(int? id) 
        {
            var entities = _repo.Get<Course>(c => c.Lecturer, c => c.College, c => c.Programme).Where(c => c.CourseId == id).ToList();
            var model = entities.Select(c => new CourseModel(c)
            {
                Lecturer = new LecturerModel(c.Lecturer),
                College = new CollegeModel(c.College),
                Programme = new ProgrammeModel(c.Programme)
            }).FirstOrDefault();
            return model;
        }

        public CourseModel[] GetCourses()
        {
            var entities = _repo.Get<Course>(c => c.Lecturer, c => c.College, c => c.Programme ).ToList();
            var models = entities.Select(c => new CourseModel(c)
            {
                Lecturer = new LecturerModel(c.Lecturer),
                College = new CollegeModel(c.College),
                Programme = new ProgrammeModel(c.Programme)
            }).ToArray();
            return models;
        }
        public CourseModel[] GetCourses(int collegeId, int programmeId, int levelId, string semester)
        {
            var entities = _repo
                .Get<Course>(c => c.Lecturer, c => c.College, c => c.Programme)
                .Where(c => c.CollegeId == collegeId && 
                   c.ProgrammeId == programmeId &&
                   c.LevelId == levelId &&
                   c.Semester == semester
                )
                .ToList();
            var models = entities.Select(c => new CourseModel(c)
            {
                Lecturer = new LecturerModel(c.Lecturer),
                College = new CollegeModel(c.College),
                Programme = new ProgrammeModel(c.Programme)
            }).ToArray();
            return models;
        }

        public void RemoveCourse(int id)
        {
            var delete = _repo.Get<Course>(id);
            _repo.Remove<Course>(id);
        }

        public void Update(int id, CourseModel model)
        {
            var course = _repo.Get<Course>(model.CourseId);
            var entity = model.Edit(course, model);
            _repo.Update<Course>(model.CourseId, entity);
        }
        #endregion

        #region Lecturer
        public void Add(LecturerModel model)
        {
            var entity = model.Create(model);
            _repo.Add<Lecturer>(entity);
        }

        public LecturerModel GetLecturer(int? id)
        {
            var entities = _repo.Get<Lecturer>(l => l.College, l => l.Programme).Where(l => l.LecturerId == id).ToList();
            var model = entities.Select(l => new LecturerModel(l)
            {
                College = new CollegeModel(l.College),
                Programme = new ProgrammeModel(l.Programme)
            }).FirstOrDefault();
            return model;
        }

        public LecturerModel[] GetLecturers()
        {
            var entities = _repo.Get<Lecturer>(l => l.Programme, l => l.College).ToList();
            var models = entities.Select(l => new LecturerModel(l)
            {
                Programme = new ProgrammeModel(l.Programme),
                College = new CollegeModel(l.College)
            }).ToArray();
            return models;
        }

        public void RemoveLecturer(int id)
        {
            var delete = _repo.Get<Lecturer>(id);
            _repo.Remove<Lecturer>(id);
        }

        public void Update(int id, LecturerModel model)
        {
            var lecturer = _repo.Get<Lecturer>(model.LecturerId);
            var entity = model.Edit(lecturer, model);
            _repo.Update(model.LecturerId, entity);
        }
        #endregion

        #region Level
        public void Add(LevelModel model)
        {
            var entity = model.Create(model);
            _repo.Add<Level>(entity);
        }

        public LevelModel GetLevel(int? id)
        {
            var model = _repo.Get<Level>(id);
            return new LevelModel(model);
        }

        public LevelModel[] GetLevels()
        {
            var entities = _repo.Get<Level>();
            var models = entities.Select(l => new LevelModel(l)).ToArray();
            return models;
        }

        public void RemoveLevel(int id)
        {
            var entity = _repo.Get<Level>(id);
            _repo.Remove<Level>(id);
        }

        public void Update(int id, LevelModel model)
        {
            var level = _repo.Get<Level>(model.LevelId);
            var entity = model.Edit(level, model);
            _repo.Update<Level>(model.LevelId, entity);
        }
        #endregion

        #region Programme
        public void Add(ProgrammeModel model)
        {
            var entity = model.Create(model);
            _repo.Add<Programme>(entity);
        }

        public ProgrammeModel GetProgramme(int? id)
        {
            var model = _repo.Get<Programme>(id);
            return new ProgrammeModel(model);
        }

        public ProgrammeModel[] GetProgrammes()
        {
            var entities = _repo.Get<Programme>();
            var models = entities.Select(p => new ProgrammeModel(p)).ToArray();
            return models;
        }

        public ProgrammeModel[] GetProgrammeByCollegeId(int collegeId)
        {
            var entities = _repo.Get<Programme>().Where(p => p.CollegeId == collegeId).ToList();
            var models = entities.Select(p => new ProgrammeModel(p)).ToArray();
            return models;
        }

        public void RemoveProgramme(int id)
        {
            var entity = _repo.Get<Programme>(id);
            _repo.Remove<Programme>(id);
        }

        public void Update(int id, ProgrammeModel model)
        {
            var program = _repo.Get<Programme>(model.ProgrammeId);
            var entity = model.Edit(program, model);
            _repo.Update<Programme>(model.ProgrammeId, entity);
        }
        #endregion

        #region Student
        public void Add(StudentModel model)
        {
            var entity = model.Create(model);
            _repo.Add<Student>(entity);
        }

        public StudentModel GetStudent(int? id)
        {
            var entities = _repo.Get<Student>(s => s.Level, s => s.College, s => s.Programme).Where(c => c.StudentId == id).ToList();
            var model = entities.Select(s => new StudentModel(s)
            {
                Level = new LevelModel(s.Level),
                College = new CollegeModel(s.College),
                Programme = new ProgrammeModel(s.Programme)
            }).FirstOrDefault();
            return model;
        }

        public StudentModel[] GetStudents()
        {
            var entities = _repo.Get<Student>(s => s.Programme, s => s.College, s => s.Level ).ToList();
            var models = entities.Select(s => new StudentModel(s)
            {
                Programme = new ProgrammeModel(s.Programme),
                College = new CollegeModel(s.College),
                Level = new LevelModel(s.Level)
            }).ToArray();
            return models;
        }

        public void RemoveStudent(int id)
        {
            var delete = _repo.Get<Student>(id);
            _repo.Remove<Student>(id);
        }

        public void Update(int id, StudentModel model)
        {
            var student = _repo.Get<Student>(model.StudentId);
            var entity = model.Edit(student, model);
            _repo.Update<Student>(model.StudentId, entity);
        }
        #endregion
    }
}
