using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.MessageModels;
using iUni_Workshop.Models.Skill;

namespace iUni_Workshop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<ExternalMeterial> ExternalMeterials { get; set; }
        public DbSet<JobHistory> JobHistories { get; set; }
        public DbSet<SkillForJob_Employee> SkillCertifications { get; set; }
        
        public DbSet<Employer> Employers { get; set; }
        public DbSet<ComplusoryWorkingDay> ComplusoryWorkingDays { get; set; }
        public DbSet<JobProfile> JobProfiles { get; set; }
        public DbSet<SkillForJob_Employer> SkillForJobEmployers { get; set; }

        public DbSet<Administraotr> Administraotrs { get; set; }

        public DbSet<Invatation> Invatations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Field> Fields { get; set; }
        public DbSet<Skill> Skills { get; set; }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            // Customize the ASP.NET Identity model and override the defaults if needed.
//            // For example, you can rename the ASP.NET Identity table names and more.
//            // Add your customizations after calling base.OnModelCreating(builder);
//        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SkillForJob_Employee>()
                .HasKey(c => new{c.CVId,c.SkillId});
            modelBuilder.Entity<SkillForJob_Employer>()
                .HasKey(c => new{c.JobProfileId,c.SkillId});
            modelBuilder.Entity<ComplusoryWorkingDay>()
                .HasKey(c => new{c.day,c.JobProfileId});
            modelBuilder.Entity<Invatation>()
                .HasKey(c => new{c.EmployeeId,c.EmployerId, c.JobProfileId});
            modelBuilder.Entity<Message>()
                .HasKey(c => new{c.MessageId,c.SenderId, c.ReciverId});
        }
    }
}
