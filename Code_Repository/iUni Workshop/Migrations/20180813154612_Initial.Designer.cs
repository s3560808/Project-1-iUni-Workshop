﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iUni_Workshop.Data;

namespace iUniWorkshop.Migrations
{
    [DbContext(typeof(SystemDbContext))]
    [Migration("20180813154612_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("iUni_Workshop.Models.Skill.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SkillName");

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}