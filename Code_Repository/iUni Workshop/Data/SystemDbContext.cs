//using iUni_Workshop.Models.EmployeeModels;
//using iUni_Workshop.Models.EmployerModels;
//using iUni_Workshop.Models.InvatationModel;
//using iUni_Workshop.Models.Skill;
//using Microsoft.EntityFrameworkCore;
//
//namespace iUni_Workshop.Data
//{
//    public class SystemDbContext:DbContext
//    {
//        public SystemDbContext(DbContextOptions options) : base(options)
//        {
//        }
//
//        public DbSet<Employee> Employees { get; set; }
//        public DbSet<CV> CVs { get; set; }
//        public DbSet<ExternalMeterial> ExternalMeterials { get; set; }
//        public DbSet<JobHistory> JobHistories { get; set; }
//        public DbSet<SkillCertification> SkillCertifications { get; set; }
////        public DbSet<Employer> Employers { get; set; }
////        public DbSet<JobProfile> JobProfiles { get; set; }
////        public DbSet<Invatation> Invatations { get; set; }
////        public DbSet<Skill> Skills { get; set; }
//        
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<SkillCertification>()
//                .HasKey(c => new{c.CVId,c.SkillId});
//        }
//    }
//}