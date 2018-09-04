﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iUni_Workshop.Data;

namespace iUniWorkshop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180901135650_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("iUni_Workshop.Models.AdministratorModels.Administraotr", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Administraotrs");
                });

            modelBuilder.Entity("iUni_Workshop.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.Employee", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ContactEmail");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.Property<int?>("SchoolId");

                    b.Property<string>("ShortDescription");

                    b.Property<int?>("SuburbId");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.HasIndex("SuburbId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeCV", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Details")
                        .IsRequired();

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<int>("FieldId");

                    b.Property<bool>("FindJobStatus");

                    b.Property<float>("MinSaraly");

                    b.Property<bool>("Primary");

                    b.Property<DateTime>("StartFindJobDate");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("FieldId");

                    b.ToTable("EmployeeCvs");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeExternalMeterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeCvId");

                    b.Property<string>("Link")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EmployeeCvId");

                    b.ToTable("EmployeeExternalMeterials");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeJobHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeCvId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EmployeeCvId");

                    b.ToTable("EmployeeJobHistories");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeSkill", b =>
                {
                    b.Property<int>("EmployeeCvId");

                    b.Property<int>("SkillId");

                    b.Property<string>("CertificationLink")
                        .IsRequired();

                    b.HasKey("EmployeeCvId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("EmployeeSkills");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeWorkDay", b =>
                {
                    b.Property<int>("Day");

                    b.Property<int>("EmployeeCvId");

                    b.HasKey("Day", "EmployeeCvId");

                    b.HasIndex("EmployeeCvId");

                    b.ToTable("EmployeeWorkDays");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.Employer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ABN");

                    b.Property<string>("BriefDescription");

                    b.Property<bool>("Certificated");

                    b.Property<string>("CertificatedBy");

                    b.Property<string>("ContactEmail");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("RequestCertification");

                    b.Property<int?>("SuburbId");

                    b.HasKey("Id");

                    b.HasIndex("CertificatedBy")
                        .IsUnique();

                    b.HasIndex("SuburbId");

                    b.ToTable("Employers");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerComplusoryWorkDay", b =>
                {
                    b.Property<int>("Day");

                    b.Property<int>("EmployerJobProfileId");

                    b.HasKey("Day", "EmployerJobProfileId");

                    b.HasIndex("EmployerJobProfileId");

                    b.ToTable("EmployerComplusoryWorkDays");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDateTime");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("EmployerId")
                        .IsRequired();

                    b.Property<int>("FieldId");

                    b.Property<DateTime>("LastUpdateDateTime");

                    b.Property<int?>("MaxDayForAWeek");

                    b.Property<int?>("MinDayForAWeek");

                    b.Property<bool>("RequireJobExperience");

                    b.Property<float>("Salary");

                    b.Property<int?>("SuburbId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EmployerId");

                    b.HasIndex("FieldId");

                    b.HasIndex("SuburbId");

                    b.ToTable("EmployerJobProfiles");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerRequiredSchool", b =>
                {
                    b.Property<int>("EmployerJobProfileId");

                    b.Property<int>("SchoolId");

                    b.HasKey("EmployerJobProfileId", "SchoolId");

                    b.HasIndex("SchoolId");

                    b.ToTable("EmployerRequiredSchools");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerSkill", b =>
                {
                    b.Property<int>("EmployerJobProfileId");

                    b.Property<int>("SkillId");

                    b.Property<bool>("Required");

                    b.HasKey("EmployerJobProfileId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("EmployerSkills");
                });

            modelBuilder.Entity("iUni_Workshop.Models.InvatationModel.Invatation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeCvId");

                    b.Property<int>("EmployerJobProfileId");

                    b.Property<DateTime>("SentTime");

                    b.Property<int>("status");

                    b.HasKey("Id");

                    b.HasIndex("EmployerJobProfileId");

                    b.HasIndex("EmployeeCvId", "EmployerJobProfileId")
                        .IsUnique();

                    b.ToTable("Invatations");
                });

            modelBuilder.Entity("iUni_Workshop.Models.JobRelatedModels.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("NewRequest");

                    b.Property<string>("NormalizedName");

                    b.Property<string>("RequestedBy");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("AddedBy")
                        .IsUnique();

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.HasIndex("RequestedBy");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("iUni_Workshop.Models.JobRelatedModels.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("NewRequest");

                    b.Property<string>("NormalizedName")
                        .IsRequired();

                    b.Property<string>("RequestedByUserId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("AddedBy")
                        .IsUnique();

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.HasIndex("RequestedByUserId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("iUni_Workshop.Models.MessageModels.Message", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConversationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("InvatationId");

                    b.Property<string>("MessageDetail")
                        .IsRequired();

                    b.Property<bool>("Read");

                    b.Property<string>("ReciverId")
                        .IsRequired();

                    b.Property<string>("SenderId")
                        .IsRequired();

                    b.Property<DateTime>("SentTime");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("InvatationId");

                    b.HasIndex("ReciverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("iUni_Workshop.Models.SchoolModels.School", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<string>("DomainExtension")
                        .IsRequired();

                    b.Property<bool>("NewRequest");

                    b.Property<string>("NormalizedName")
                        .IsRequired();

                    b.Property<string>("RequestedBy");

                    b.Property<string>("SchoolName")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<int>("SuburbId");

                    b.HasKey("Id");

                    b.HasIndex("AddedBy");

                    b.HasIndex("RequestedBy");

                    b.HasIndex("SuburbId");

                    b.HasIndex("DomainExtension", "NormalizedName", "SuburbId")
                        .IsUnique();

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("iUni_Workshop.Models.StateModels.State", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("States");

                    b.HasData(
                        new { Id = 1, Name = "ACT" },
                        new { Id = 2, Name = "NSW" },
                        new { Id = 3, Name = "NT" },
                        new { Id = 4, Name = "QLD" },
                        new { Id = 5, Name = "SA" },
                        new { Id = 6, Name = "TAS" },
                        new { Id = 7, Name = "VIC" },
                        new { Id = 8, Name = "WA" },
                        new { Id = 9, Name = "JBT" }
                    );
                });

            modelBuilder.Entity("iUni_Workshop.Models.SuburbModels.Suburb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInfo");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("PostCode");

                    b.Property<int>("StateId");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("Suburbs");
                });

            modelBuilder.Entity("iUniWorkshop.Models.EmployerModels.EmployerRequiredWorkLocation", b =>
                {
                    b.Property<int>("EmployerJobProfileId");

                    b.Property<int>("SuburbId");

                    b.HasKey("EmployerJobProfileId", "SuburbId");

                    b.HasIndex("SuburbId");

                    b.ToTable("EmployerWorkLocations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("iUni_Workshop.Models.AdministratorModels.Administraotr", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.Employee", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.SchoolModels.School", "School")
                        .WithMany("Employee")
                        .HasForeignKey("SchoolId");

                    b.HasOne("iUni_Workshop.Models.SuburbModels.Suburb", "Suburb")
                        .WithMany("Employees")
                        .HasForeignKey("SuburbId");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeCV", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.Employee", "Employee")
                        .WithMany("EmployeeCvs")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.JobRelatedModels.Field", "Field")
                        .WithMany("EmployeeCvs")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeExternalMeterial", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.EmployeeCV", "EmployeeCV")
                        .WithMany("EmployeeExternalMeterials")
                        .HasForeignKey("EmployeeCvId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeJobHistory", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.EmployeeCV", "EmployeeCV")
                        .WithMany("EmployeeJobHistories")
                        .HasForeignKey("EmployeeCvId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeSkill", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.EmployeeCV", "EmployeeCV")
                        .WithMany("EmployeeSkills")
                        .HasForeignKey("EmployeeCvId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.JobRelatedModels.Skill", "Skill")
                        .WithMany("EmployeeSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.EmployeeWorkDay", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.EmployeeCV", "EmployeeCv")
                        .WithMany("EmployeeWorkDays")
                        .HasForeignKey("EmployeeCvId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.Employer", b =>
                {
                    b.HasOne("iUni_Workshop.Models.AdministratorModels.Administraotr", "Administraotr")
                        .WithOne("Employer")
                        .HasForeignKey("iUni_Workshop.Models.EmployerModels.Employer", "CertificatedBy");

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.SuburbModels.Suburb", "Suburb")
                        .WithMany("Employers")
                        .HasForeignKey("SuburbId");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerComplusoryWorkDay", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", "EmployerJobProfile")
                        .WithMany("EmployerComplusoryWorkDays")
                        .HasForeignKey("EmployerJobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.Employer", "Employer")
                        .WithMany()
                        .HasForeignKey("EmployerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.JobRelatedModels.Field", "Field")
                        .WithMany("EmployerJobProfiles")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.SuburbModels.Suburb")
                        .WithMany("EmployerJobProfile")
                        .HasForeignKey("SuburbId");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerRequiredSchool", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", "EmployerJobProfile")
                        .WithMany("EmployerRequiredSchools")
                        .HasForeignKey("EmployerJobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.SchoolModels.School", "School")
                        .WithMany("EmployerRequiredSchools")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.EmployerSkill", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", "EmployerJobProfile")
                        .WithMany("EmployerSkills")
                        .HasForeignKey("EmployerJobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.JobRelatedModels.Skill", "Skill")
                        .WithMany("EmployerSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.InvatationModel.Invatation", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.EmployeeCV", "EmployeeCV")
                        .WithMany("Invatations")
                        .HasForeignKey("EmployeeCvId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", "EmployerJobProfile")
                        .WithMany()
                        .HasForeignKey("EmployerJobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.JobRelatedModels.Field", b =>
                {
                    b.HasOne("iUni_Workshop.Models.AdministratorModels.Administraotr", "Administraotr")
                        .WithOne("Field")
                        .HasForeignKey("iUni_Workshop.Models.JobRelatedModels.Field", "AddedBy");

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("RequestedBy");
                });

            modelBuilder.Entity("iUni_Workshop.Models.JobRelatedModels.Skill", b =>
                {
                    b.HasOne("iUni_Workshop.Models.AdministratorModels.Administraotr", "Administraotr")
                        .WithOne("Skill")
                        .HasForeignKey("iUni_Workshop.Models.JobRelatedModels.Skill", "AddedBy");

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("RequestedByUserId");
                });

            modelBuilder.Entity("iUni_Workshop.Models.MessageModels.Message", b =>
                {
                    b.HasOne("iUni_Workshop.Models.InvatationModel.Invatation", "Invatation")
                        .WithMany()
                        .HasForeignKey("InvatationId");

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReciverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.SchoolModels.School", b =>
                {
                    b.HasOne("iUni_Workshop.Models.AdministratorModels.Administraotr", "Administrator")
                        .WithMany("Schools")
                        .HasForeignKey("AddedBy");

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("RequestedBy");

                    b.HasOne("iUni_Workshop.Models.SuburbModels.Suburb", "Suburb")
                        .WithMany("Schools")
                        .HasForeignKey("SuburbId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.SuburbModels.Suburb", b =>
                {
                    b.HasOne("iUni_Workshop.Models.StateModels.State", "State")
                        .WithMany("Suburbs")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUniWorkshop.Models.EmployerModels.EmployerRequiredWorkLocation", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.EmployerJobProfile", "EmployerJobProfile")
                        .WithMany("EmployerRequiredWorkLocations")
                        .HasForeignKey("EmployerJobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.SuburbModels.Suburb", "Suburb")
                        .WithMany("EmployerRequiredWorkLocations")
                        .HasForeignKey("SuburbId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
