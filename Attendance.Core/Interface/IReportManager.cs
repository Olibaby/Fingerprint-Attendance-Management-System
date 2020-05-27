using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Interface
{
    public interface IReportManager
    {
        List<dynamic> getAttendanceReport(int collegeId, int programmeId, int levelId, string semester , int courseId);
    }
}
