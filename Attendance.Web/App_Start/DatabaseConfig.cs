using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Attendance.Web.App_Start
{
    public class DatabaseConfig
    {
        public static void MigrateToLatest()
        {
            //Upgrade DB to Latest
            var configuration = new Data.Migrations.Configuration
            {
                //Allow for Data Loss Migrations
                AutomaticMigrationDataLossAllowed = true
            };

            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}