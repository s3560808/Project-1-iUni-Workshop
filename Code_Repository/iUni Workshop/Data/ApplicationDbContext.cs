using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUniWorkshop.Data.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.MessageModels;
using iUni_Workshop.Models.SchoolModels;
using iUni_Workshop.Models.StateModels;
using iUni_Workshop.Models.SuburbModels;
using iUniWorkshop.Models.EmployerModels;
using iUni_Workshop.Data.Seeds;

namespace iUni_Workshop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeCV> EmployeeCvs { get; set; }
        public DbSet<EmployeeExternalMeterial> EmployeeExternalMeterials { get; set; }
        public DbSet<EmployeeJobHistory> EmployeeJobHistories { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public DbSet<EmployeeWorkDay> EmployeeWorkDays { get; set; }


        public DbSet<Employer> Employers { get; set; }
        public DbSet<EmployerComplusoryWorkDay> EmployerComplusoryWorkDays { get; set; }
        public DbSet<EmployerJobProfile> EmployerJobProfiles { get; set; }
        public DbSet<EmployerSkill> EmployerSkills { get; set; }
        public DbSet<EmployerRequiredSchool> EmployerRequiredSchools { get; set; }
        public DbSet<EmployerRequiredWorkLocation> EmployerWorkLocations { get; set; }

        public DbSet<Administraotr> Administraotrs { get; set; }

        public DbSet<Invatation> Invatations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<FieldHistory> FieldHistories { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<SkillHistory> SkillHistories { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DbSet<School> Schools { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Suburb> Suburbs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Set Composite key
            
            modelBuilder.Entity<EmployeeWorkDay>()
                .HasKey(c => new { c.Day, c.EmployeeCvId });
            
            modelBuilder.Entity<EmployerComplusoryWorkDay>()
                .HasKey(c => new { c.Day, c.EmployerJobProfileId });
            modelBuilder.Entity<EmployerRequiredSchool>()
                .HasKey(c => new { c.EmployerJobProfileId, c.SchoolId });
            modelBuilder.Entity<EmployerRequiredWorkLocation>()
                        .HasKey(c => new { c.EmployerJobProfileId, c.SuburbId });


            //Set unique key
            modelBuilder.Entity<Skill>()
                .HasIndex(b => b.NormalizedName)
                .IsUnique();
            modelBuilder.Entity<Field>()
                .HasIndex(b => b.NormalizedName)
                .IsUnique();
            modelBuilder.Entity<School>()
                .HasIndex(c => new { c.DomainExtension, c.NormalizedName, c.SuburbId })
                .IsUnique();
            modelBuilder.Entity<Invatation>()
                .HasIndex(c => new { c.EmployeeCvId, c.EmployerJobProfileId })
                .IsUnique();
            modelBuilder.Entity<EmployeeSkill>()
                .HasIndex(c => new { c.EmployeeCvId, c.SkillId })
                .IsUnique();
            modelBuilder.Entity<EmployerSkill>()
                .HasIndex(c => new { c.EmployerJobProfileId, c.SkillId })
                .IsUnique();

            //Seed System data
            modelBuilder.Entity<State>().HasData(StateSeed.states);
            modelBuilder.Entity<Suburb>().HasData(SuburbSeed.suburbs);
            modelBuilder.Entity<Field>().HasData(FieldSeed.fields);
            modelBuilder.Entity<Skill>().HasData(SkillSeed.skills);
            modelBuilder.Entity<School>().HasData(SchoolSeed.schools);
        }
    
    }
}
