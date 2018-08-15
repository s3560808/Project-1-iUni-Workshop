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
    [Migration("20180815035837_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.CV", b =>
                {
                    b.Property<int>("CVId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CVDetails")
                        .IsRequired();

                    b.Property<string>("CVTitle")
                        .IsRequired();

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<DateTime>("FJSSetDate");

                    b.Property<int>("FieldId");

                    b.Property<bool>("FindJobStatus");

                    b.HasKey("CVId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("FieldId")
                        .IsUnique();

                    b.ToTable("CVs");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.Employee", b =>
                {
                    b.Property<string>("EmployeeId");

                    b.Property<string>("ContactEmail");

                    b.Property<string>("LivingSuburb");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Photo");

                    b.Property<string>("SchooName");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.ExternalMeterial", b =>
                {
                    b.Property<int>("ExternalMeterialId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CVId");

                    b.Property<string>("ExternalMeterialLink")
                        .IsRequired();

                    b.Property<string>("ExternalMeterialName")
                        .IsRequired();

                    b.HasKey("ExternalMeterialId");

                    b.HasIndex("CVId");

                    b.ToTable("ExternalMeterials");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.JobHistory", b =>
                {
                    b.Property<int>("JobHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CVId");

                    b.Property<string>("PreviousJobName")
                        .IsRequired();

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.HasKey("JobHistoryId");

                    b.HasIndex("CVId");

                    b.ToTable("JobHistories");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.SkillForJob_Employee", b =>
                {
                    b.Property<int>("CVId");

                    b.Property<int>("SkillId");

                    b.Property<string>("CertificationLink")
                        .IsRequired();

                    b.HasKey("CVId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("SkillCertifications");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.ComplusoryWorkingDay", b =>
                {
                    b.Property<int>("day");

                    b.Property<int>("JobProfileId");

                    b.HasKey("day", "JobProfileId");

                    b.HasIndex("JobProfileId");

                    b.ToTable("ComplusoryWorkingDays");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.Employer", b =>
                {
                    b.Property<string>("EmployerId");

                    b.Property<string>("ABN")
                        .IsRequired();

                    b.Property<string>("BriefDescription")
                        .IsRequired();

                    b.Property<bool>("Certificated");

                    b.Property<string>("CompanyLocation")
                        .IsRequired();

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<string>("ContactEmail")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("Photo")
                        .IsRequired();

                    b.HasKey("EmployerId");

                    b.ToTable("Employers");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.JobProfile", b =>
                {
                    b.Property<int>("JobProfileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmployerId")
                        .IsRequired();

                    b.Property<DateTime>("EstablishDateTime");

                    b.Property<string>("JobDescription")
                        .IsRequired();

                    b.Property<string>("JobTitleP")
                        .IsRequired();

                    b.Property<int>("MaxDayForAWeek");

                    b.Property<int>("MinDayForAWeek");

                    b.Property<bool>("RequireJobExperience");

                    b.Property<double>("Salary");

                    b.Property<string>("SalaryUnit")
                        .IsRequired();

                    b.HasKey("JobProfileId");

                    b.HasIndex("EmployerId");

                    b.ToTable("JobProfiles");
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.SkillForJob_Employer", b =>
                {
                    b.Property<int>("JobProfileId");

                    b.Property<int>("SkillId");

                    b.Property<bool>("required");

                    b.HasKey("JobProfileId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("SkillForJobEmployers");
                });

            modelBuilder.Entity("iUni_Workshop.Models.InvatationModel.Invatation", b =>
                {
                    b.Property<string>("EmployeeId");

                    b.Property<string>("EmployerId");

                    b.Property<string>("JobProfileId");

                    b.Property<int?>("JobFrofileId");

                    b.Property<DateTime>("SentDate");

                    b.HasKey("EmployeeId", "EmployerId", "JobProfileId");

                    b.HasIndex("EmployerId");

                    b.HasIndex("JobFrofileId");

                    b.ToTable("Invatations");
                });

            modelBuilder.Entity("iUni_Workshop.Models.JobRelatedModels.Field", b =>
                {
                    b.Property<int>("FieldId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FieldName");

                    b.HasKey("FieldId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("iUni_Workshop.Models.MessageModels.Message", b =>
                {
                    b.Property<string>("MessageId");

                    b.Property<string>("SenderId");

                    b.Property<string>("ReciverId");

                    b.Property<int>("JobProfileId");

                    b.Property<DateTime>("SentTime");

                    b.Property<bool>("read");

                    b.HasKey("MessageId", "SenderId", "ReciverId");

                    b.HasAlternateKey("JobProfileId", "MessageId", "ReciverId", "SenderId");

                    b.HasIndex("ReciverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("iUni_Workshop.Models.Skill.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SkillName");

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
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

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.CV", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.Employee", "Employee")
                        .WithMany("CVs")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.JobRelatedModels.Field", "Field")
                        .WithOne("CV")
                        .HasForeignKey("iUni_Workshop.Models.EmployeeModels.CV", "FieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.Employee", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Employee")
                        .HasForeignKey("iUni_Workshop.Models.EmployeeModels.Employee", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.ExternalMeterial", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.CV", "CV")
                        .WithMany("ExternalMeterials")
                        .HasForeignKey("CVId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.JobHistory", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.CV", "CV")
                        .WithMany("JobHisotries")
                        .HasForeignKey("CVId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployeeModels.SkillForJob_Employee", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.CV", "CV")
                        .WithMany("SkillForJobEmployees")
                        .HasForeignKey("CVId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.Skill.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.ComplusoryWorkingDay", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.JobProfile", "JobProfile")
                        .WithMany("ComplusoryWorkingDay")
                        .HasForeignKey("JobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.Employer", b =>
                {
                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("EmployerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.JobProfile", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.Employer", "Employer")
                        .WithMany()
                        .HasForeignKey("EmployerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.EmployerModels.SkillForJob_Employer", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.JobProfile", "JobProfile")
                        .WithMany("SkillForJobEmployers")
                        .HasForeignKey("JobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.Skill.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("iUni_Workshop.Models.InvatationModel.Invatation", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployeeModels.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.EmployerModels.Employer", "Employer")
                        .WithMany()
                        .HasForeignKey("EmployerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.EmployerModels.JobProfile", "JobProfile")
                        .WithMany()
                        .HasForeignKey("JobFrofileId");
                });

            modelBuilder.Entity("iUni_Workshop.Models.MessageModels.Message", b =>
                {
                    b.HasOne("iUni_Workshop.Models.EmployerModels.JobProfile", "JobProfile")
                        .WithMany()
                        .HasForeignKey("JobProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReciverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("iUni_Workshop.Models.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
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
