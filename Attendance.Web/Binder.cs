using Attendance.Core.Interface;
using Attendance.Core.Manager;
using Attendance.Data;
using Attendance.Data.Repository;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Attendance.Web
{
    public class Binder : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<DbContext>().To<DataEntity>().InRequestScope();
            Bind<IGenericRepository>().To<GenericRepository>().InRequestScope();
            Bind<IAttendanceManager>().To<AttendanceManager>().InRequestScope();
            Bind<IUserManager>().To<UserManager>().InRequestScope();
            Bind<IReportManager>().To<ReportManager>().InRequestScope();
        }
    }
}