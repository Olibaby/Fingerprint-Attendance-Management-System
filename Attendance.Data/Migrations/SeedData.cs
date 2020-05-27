using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data.Migrations
{
    public class SeedData
    {
        public static List<College> SeedColleges(DataEntity context)
        {
            var colleges = new List<College>
            {
                new College() { CollegeName = "College of Business and Social Sciences" },
                new College() { CollegeName = "College of Leadership Development Studies" },
                new College() { CollegeName = "College of Engineering" },
                new College() { CollegeName = "College of Science and Technology" },
            };
            colleges.ForEach(college =>
            {
                if (!context.Colleges.Where(c => c.CollegeName == college.CollegeName).Any())
                    context.Colleges.Add(college);
            });
            context.SaveChanges();
            return colleges;
        }

        public static List<Level> SeedLevels(DataEntity context)
        {
            var levels = new List<Level>
            {
                new Level{ LevelName = "100" },
                new Level{ LevelName = "200" },
                new Level{ LevelName = "300" },
                new Level{ LevelName = "400" },
                new Level{ LevelName = "500" },
            };
            levels.ForEach(level =>
            {
                if (!context.Levels.Where(c => c.LevelName == level.LevelName).Any())
                    context.Levels.Add(level);
            });

            context.SaveChanges();
            return levels;
        }
        public static List<Programme> SeedProgrammes(DataEntity context)
        {
            var bsscollege = context.Colleges.Where(c => c.CollegeName == "College of Business and Social Sciences").FirstOrDefault();
            var ldscollege = context.Colleges.Where(c => c.CollegeName == "College of Leadership Development Studies").FirstOrDefault();
            var engcollege = context.Colleges.Where(c => c.CollegeName == "College of Engineering").FirstOrDefault();
            var sntcollege = context.Colleges.Where(c => c.CollegeName == "College of Science and Technology").FirstOrDefault();

            var programmes = new List<Programme>
            {
                new Programme { ProgrammeName = "Accounting", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Banking and Finance", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Business Administration", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Industrial Relations and Human Resource Management", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Marketing", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Entrepreneurship", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Demography and Social Statistics", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Economics", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Mass Communication", CollegeId = bsscollege.CollegeId },
                new Programme { ProgrammeName = "Sociology", CollegeId = bsscollege.CollegeId },

                new Programme { ProgrammeName = "Marketing", CollegeId = ldscollege.CollegeId },
                new Programme { ProgrammeName = "Entrepreneurship", CollegeId = ldscollege.CollegeId },
                new Programme { ProgrammeName = "Demography and Social Statistics", CollegeId = ldscollege.CollegeId },
                new Programme { ProgrammeName = "Economics", CollegeId = ldscollege.CollegeId },
                new Programme { ProgrammeName = "Mass Communication", CollegeId = ldscollege.CollegeId },
                new Programme { ProgrammeName = "Sociology", CollegeId = ldscollege.CollegeId },

                new Programme { ProgrammeName = "Civil Engineering", CollegeId = engcollege.CollegeId },
                new Programme { ProgrammeName = "Computer Engineering", CollegeId = engcollege.CollegeId },
                new Programme { ProgrammeName = "Electrical and Electronics Engineering", CollegeId = engcollege.CollegeId },
                new Programme { ProgrammeName = "Information and Communication Engineering", CollegeId = engcollege.CollegeId },
                new Programme { ProgrammeName = "Mechanical Engineering", CollegeId = engcollege.CollegeId },
                new Programme { ProgrammeName = "Petroleum Engineering", CollegeId = engcollege.CollegeId },
                new Programme { ProgrammeName = "Chemical Engineering", CollegeId = engcollege.CollegeId },

                new Programme { ProgrammeName = "Architecture", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Building Technology", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Estate Management", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Applied Biology and Biotechnology", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Biochemistry", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Microbiology", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Chemistry", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Computer Science", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Management Information System", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Industrial Mathematics", CollegeId = sntcollege.CollegeId },
                new Programme { ProgrammeName = "Industrial Physics", CollegeId = sntcollege.CollegeId },
            };
            programmes.ForEach(programme =>
            {
                if (!context.Programmes.Where(p => p.ProgrammeName == programme.ProgrammeName).Any())
                    context.Programmes.Add(programme);
                
            });
            context.SaveChanges();
            return programmes;
        }

        public static List<Lecturer> SeedLecturers(DataEntity context)
        {
            var cengprogramme = context.Programmes.Where(c => c.ProgrammeName == "Computer Engineering").FirstOrDefault();

            var lecturers = new List<Lecturer>
            {
                new Lecturer { Title = "Dr", FirstName = "Hope", LastName = "Orovwode", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "hope@convenat.com", Gender = "Female" },
                new Lecturer { Title ="Dr", FirstName = "Victoria", LastName = "Oguntosi", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "victoria@convenat.com", Gender = "Female" },

                new Lecturer { Title ="Engr", FirstName = "Kennedy", LastName = "Okokpujie", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "kennedy@convenat.com", Gender = "Male"},
                new Lecturer { Title ="Dr", FirstName = "Charles", LastName = "Ndujiuba", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "charles@convenat.com", Gender = "Male"},
                new Lecturer { Title ="Engr", FirstName = "Nsikan", LastName = "Nkerdeh", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "nsikan@convenat.com", Gender = "Male"  },
                new Lecturer { Title ="Engr", FirstName = "Osemwegie", LastName = "Omoruyi", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "omoruyi@convenat.com", Gender = "Male"  },
                new Lecturer { Title ="Dr", FirstName = "Sanjay",LastName = "Misra", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "anjay@convenat.com", Gender = "Female" },
                new Lecturer { Title ="Dr", FirstName = "Joke", LastName = "Badejo", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "joke@convenat.com", Gender = "Female"},
                new Lecturer { Title ="Prof", FirstName = "Samuel", LastName = "Ndueso", MiddleName = "John", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "samuel@convenat.com", Gender = "Male"},
                new Lecturer { Title ="Engr", FirstName = "Modupe", LastName = "Odusami", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "modupe@convenat.com", Gender = "Female" },

                new Lecturer { Title ="Mrs", FirstName = "Onyinyechi", LastName = "Steve-Essi", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "onyinyechi@convenat.com", Gender = "Male"  },
                new Lecturer { Title ="Mr", FirstName = "Jeremiah", LastName = "Abolade", MiddleName = "", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Email = "eremiah@convenat.com", Gender = "Male"  },
                
            };

            lecturers.ForEach(lecturer =>
            {
                if (!context.Lecturers.Where(c => c.FirstName == lecturer.FirstName && c.LastName == lecturer.LastName).Any())
                {
                    context.Lecturers.Add(lecturer);
                }
            });
            context.SaveChanges();
            return lecturers;
        }
        public static List<Course> SeedCourses(DataEntity context)
        {
            var cengprogramme = context.Programmes.Where(c => c.ProgrammeName == "Computer Engineering").FirstOrDefault();

            var courses = new List<Course>
            {
                new Course { CourseCode = "CEN510", CourseName = "Digital System Design with VHDL", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5 },
                new Course { CourseCode = "CEN511", CourseName = "Embedded System Design & Programming", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5 },
                new Course { CourseCode = "DLD211", CourseName = "Leadership Development: Contextual Platform", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5},
                new Course { CourseCode = "EDS511", CourseName = "Entrepreneurial Development Studies IX", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5},

                new Course { CourseCode = "EIE510", CourseName = "Research Methodology", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5  },
                new Course { CourseCode = "EIE511", CourseName = "Project Management", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5  },
                new Course { CourseCode = "EIE512", CourseName = "Systems Reliability and Maintainability", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5},
                new Course { CourseCode = "EIE513", CourseName = "Cyberpreneurship and Cyber law", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5},
                new Course { CourseCode = "EIE515", CourseName = "Digital Signal Processing", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5},
                new Course { CourseCode = "EIE517", CourseName = "Applied Electronics", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5 },
                new Course { CourseCode = "TMC511", CourseName = "Total Man Concept IX", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5},
                new Course { CourseCode = "TMC512", CourseName = "Total Man Concept - Sports", CollegeId = cengprogramme.CollegeId, ProgrammeId = cengprogramme.ProgrammeId, Semester ="First", LevelId = 5 },
            };

            courses.ForEach(course =>
            {
                if (!context.Courses.Where(c => c.CourseName == course.CourseName).Any())
                {
                    /*course.College = null;
                    course.Programme = null;
                    course.Lecturer = null;
                    course.Level = null;*/
                    context.Courses.Add(course);
                    
                }
            });
            context.SaveChanges();
            return courses;
        }
        public static List<User> SeedUsers(DataEntity context)
        {
            var courses = new List<User>
            {
                new User { UserName = "Admin", Password = "Admin" },
                new User { UserName = "Lecturer", Password = "Lecturer" },
                new User { UserName = "Student",Password = "Student"  },
            };
            courses.ForEach(user =>
            {
                if (!context.Users.Where(u => u.UserName == user.UserName).Any())
                    context.Users.Add(user);
            });
            context.SaveChanges();
            return courses;
        }
        public static List<Role> SeedRoles(DataEntity context)
        {
            var courses = new List<Role>
            {
                new Role { RoleName = "Admin" },
                new Role { RoleName = "Lecturer" },
                new Role { RoleName = "Student" },
            };
            courses.ForEach(role =>
            {
                if (!context.Roles.Where(r => r.RoleName == role.RoleName).Any())
                    context.Roles.Add(role);
            });
            context.SaveChanges();
            return courses;
        }
        public static List<UserRole> SeedUserRoles(DataEntity context)
        {
            var userroles = new List<UserRole>
            {
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 2, RoleId = 2 },
                new UserRole { UserId = 3, RoleId = 3 },
            };
            userroles.ForEach(userrole =>
            {
                if (!context.UserRoles.Where(ur => ur.UserId == userrole.UserId && ur.RoleId == userrole.RoleId).Any())
                    context.UserRoles.Add(userrole);
            });
            context.SaveChanges();
            return userroles;
        }
    }
}
