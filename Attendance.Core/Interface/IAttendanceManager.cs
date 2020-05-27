using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Interface
{
    public interface IAttendanceManager
    {
        #region College
        CollegeModel[] GetColleges();
        CollegeModel GetCollege(int? id);
        void Add(CollegeModel model);
        void Update(int id, CollegeModel model);
        void RemoveCollege(int id);
        #endregion

        #region Course
        CourseModel[] GetCourses();
        CourseModel[] GetCourses(int collegeId, int programmeId, int levelId, string semester);
        CourseModel GetCourse(int? id);
        void Add(CourseModel model);
        void Update(int id, CourseModel model);
        void RemoveCourse(int id);
        #endregion 

        #region Lecturer
        LecturerModel[] GetLecturers();
        LecturerModel GetLecturer(int? id);
        void Add(LecturerModel model);
        void Update(int id, LecturerModel model);
        void RemoveLecturer(int id); 
        #endregion

        #region Level
        LevelModel[] GetLevels();
        LevelModel GetLevel(int? id);
        void Add(LevelModel model);
        void Update(int id, LevelModel model);
        void RemoveLevel(int id);
        #endregion

        #region Program
        ProgrammeModel[] GetProgrammes();
        ProgrammeModel[] GetProgrammeByCollegeId(int collegeId);
        ProgrammeModel GetProgramme(int? id);
        void Add(ProgrammeModel model);
        void Update(int id, ProgrammeModel model);
        void RemoveProgramme(int id);
        #endregion

        #region Student
        StudentModel[] GetStudents();
        StudentModel GetStudent(int? id);
        void Add(StudentModel model);
        void Update(int id, StudentModel model);
        void RemoveStudent(int id);
        #endregion
    }
}
