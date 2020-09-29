namespace Diary
{
    using Diary.Models.Configurations;
    using Diary.Models.Domains;
    using Diary.Properties;
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Windows;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base($@"Server={Settings.Default.DbServerAddress}\{Settings.Default.DbServerName};
                    Database={Settings.Default.DbName};
                    User Id={Settings.Default.DbUser};
                    Password={Settings.Default.DbPassword};")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }

    

    }
}