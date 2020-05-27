namespace Attendance.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<DataEntity>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataEntity context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //  This method will be called after migrating to the latest version.
            SeedData.SeedColleges(context);
            SeedData.SeedLevels(context);
            SeedData.SeedProgrammes(context);
            SeedData.SeedLecturers(context);
            SeedData.SeedCourses(context);
            SeedData.SeedUsers(context);
            SeedData.SeedRoles(context);
            SeedData.SeedUserRoles(context);
        }
    }
}
