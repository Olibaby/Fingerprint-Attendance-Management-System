using Attendance.Core.Interface;
using Attendance.Data.Repository;
using Attendance.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Data.Entity;
using System.Dynamic;

namespace Attendance.Core.Manager
{
    public class ReportManager : IReportManager
    {
        private IGenericRepository _db;
        public ReportManager(IGenericRepository db)
        {
            _db = db;
        }
        public List<dynamic> getAttendanceReport(int collegeId, int programmeId, int levelId, string semester, int courseId)
        {
            DataTable dataTable = new DataTable();
            var sql =
                "SELECT DISTINCT convert(Date,AttendanceDate) as AttendanceDate INTO #Dates FROM Attendances Where CollegeId = " + collegeId + " and ProgrammeId = " + programmeId + " and CourseId = " + courseId + " and Semester = '" + semester + "' ORDER BY AttendanceDate" +
                " DECLARE @cols varchar(1000)" +
                "set @cols = ''" +
                "SELECT @cols = @cols + 'SUM(CASE Convert(Date,AttendanceDate) WHEN ' + ' '' ' + convert(varchar(25),AttendanceDate) + ' '' THEN 1 ELSE 0 END ) '' ' + convert(varchar(25),AttendanceDate) + ''',' FROM #Dates" +
                " DECLARE @qry varchar(4000)" +
                " SET @qry = 'SELECT stud.LastName, stud.FirstName,stud.MiddleName, stud.MatricNo, ' +  @cols + ' stud.ProgrammeId" +
                " FROM Attendances attnd " +
                " RIGHT OUTER JOIN Students stud" +
                " ON          stud.MatricNo = attnd.MatricNo" +
                " GROUP BY    stud.MatricNo,stud.LastName, stud.FirstName,stud.MiddleName,stud.ProgrammeId" +
                " ORDER BY    stud.MatricNo" +
                "'" +
                " EXEC(@qry)" +
                " DROP TABLE #Dates";


            List<dynamic> results = Extension.DynamicListFromSql(new DataEntity(),sql, new Dictionary<string, object> { { "a", true }, { "b", false } }).ToList();

            return results.ToList();
        }
    }
}
