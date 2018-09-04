using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iUniWorkshop.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    AccessFailedCount = table.Column<int>(nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Administraotrs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administraotrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administraotrs_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suburbs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    PostCode = table.Column<int>(nullable: false),
                    AdditionalInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suburbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suburbs_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    NewRequest = table.Column<bool>(nullable: false),
                    RequestedBy = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fields_Administraotrs_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "Administraotrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fields_AspNetUsers_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    NewRequest = table.Column<bool>(nullable: false),
                    RequestedByUserId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Administraotrs_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "Administraotrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    SuburbId = table.Column<int>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    BriefDescription = table.Column<string>(nullable: true),
                    ABN = table.Column<string>(nullable: true),
                    Certificated = table.Column<bool>(nullable: false),
                    RequestCertification = table.Column<bool>(nullable: false),
                    CertificatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employers_Administraotrs_CertificatedBy",
                        column: x => x.CertificatedBy,
                        principalTable: "Administraotrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employers_Suburbs_SuburbId",
                        column: x => x.SuburbId,
                        principalTable: "Suburbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DomainExtension = table.Column<string>(nullable: false),
                    SchoolName = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: false),
                    SuburbId = table.Column<int>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    NewRequest = table.Column<bool>(nullable: false),
                    RequestedBy = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Administraotrs_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "Administraotrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_AspNetUsers_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_Suburbs_SuburbId",
                        column: x => x.SuburbId,
                        principalTable: "Suburbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployerJobProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployerId = table.Column<string>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateDateTime = table.Column<DateTime>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    RequireJobExperience = table.Column<bool>(nullable: false),
                    MaxDayForAWeek = table.Column<int>(nullable: true),
                    MinDayForAWeek = table.Column<int>(nullable: true),
                    Salary = table.Column<float>(nullable: false),
                    SuburbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerJobProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployerJobProfiles_Employers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerJobProfiles_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerJobProfiles_Suburbs_SuburbId",
                        column: x => x.SuburbId,
                        principalTable: "Suburbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchoolId = table.Column<int>(nullable: true),
                    SuburbId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Suburbs_SuburbId",
                        column: x => x.SuburbId,
                        principalTable: "Suburbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployerComplusoryWorkDays",
                columns: table => new
                {
                    EmployerJobProfileId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerComplusoryWorkDays", x => new { x.Day, x.EmployerJobProfileId });
                    table.ForeignKey(
                        name: "FK_EmployerComplusoryWorkDays_EmployerJobProfiles_EmployerJobPr~",
                        column: x => x.EmployerJobProfileId,
                        principalTable: "EmployerJobProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployerRequiredSchools",
                columns: table => new
                {
                    EmployerJobProfileId = table.Column<int>(nullable: false),
                    SchoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerRequiredSchools", x => new { x.EmployerJobProfileId, x.SchoolId });
                    table.ForeignKey(
                        name: "FK_EmployerRequiredSchools_EmployerJobProfiles_EmployerJobProfi~",
                        column: x => x.EmployerJobProfileId,
                        principalTable: "EmployerJobProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerRequiredSchools_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployerSkills",
                columns: table => new
                {
                    EmployerJobProfileId = table.Column<int>(nullable: false),
                    SkillId = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerSkills", x => new { x.EmployerJobProfileId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_EmployerSkills_EmployerJobProfiles_EmployerJobProfileId",
                        column: x => x.EmployerJobProfileId,
                        principalTable: "EmployerJobProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployerWorkLocations",
                columns: table => new
                {
                    SuburbId = table.Column<int>(nullable: false),
                    EmployerJobProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerWorkLocations", x => new { x.EmployerJobProfileId, x.SuburbId });
                    table.ForeignKey(
                        name: "FK_EmployerWorkLocations_EmployerJobProfiles_EmployerJobProfile~",
                        column: x => x.EmployerJobProfileId,
                        principalTable: "EmployerJobProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerWorkLocations_Suburbs_SuburbId",
                        column: x => x.SuburbId,
                        principalTable: "Suburbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCvs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: false),
                    Primary = table.Column<bool>(nullable: false),
                    FindJobStatus = table.Column<bool>(nullable: false),
                    StartFindJobDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    MinSaraly = table.Column<float>(nullable: false),
                    FieldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCvs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCvs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeCvs_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeExternalMeterials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    EmployeeCvId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeExternalMeterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeExternalMeterials_EmployeeCvs_EmployeeCvId",
                        column: x => x.EmployeeCvId,
                        principalTable: "EmployeeCvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeJobHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    ShortDescription = table.Column<string>(nullable: false),
                    EmployeeCvId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeJobHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeJobHistories_EmployeeCvs_EmployeeCvId",
                        column: x => x.EmployeeCvId,
                        principalTable: "EmployeeCvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkills",
                columns: table => new
                {
                    SkillId = table.Column<int>(nullable: false),
                    EmployeeCvId = table.Column<int>(nullable: false),
                    CertificationLink = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkills", x => new { x.EmployeeCvId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_EmployeeSkills_EmployeeCvs_EmployeeCvId",
                        column: x => x.EmployeeCvId,
                        principalTable: "EmployeeCvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkDays",
                columns: table => new
                {
                    EmployeeCvId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkDays", x => new { x.Day, x.EmployeeCvId });
                    table.ForeignKey(
                        name: "FK_EmployeeWorkDays_EmployeeCvs_EmployeeCvId",
                        column: x => x.EmployeeCvId,
                        principalTable: "EmployeeCvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invatations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeCvId = table.Column<int>(nullable: false),
                    EmployerJobProfileId = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    SentTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invatations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invatations_EmployeeCvs_EmployeeCvId",
                        column: x => x.EmployeeCvId,
                        principalTable: "EmployeeCvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invatations_EmployerJobProfiles_EmployerJobProfileId",
                        column: x => x.EmployerJobProfileId,
                        principalTable: "EmployerJobProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConversationId = table.Column<string>(nullable: true),
                    SenderId = table.Column<string>(nullable: false),
                    ReciverId = table.Column<string>(nullable: false),
                    InvatationId = table.Column<int>(nullable: true),
                    SentTime = table.Column<DateTime>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    MessageDetail = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Invatations_InvatationId",
                        column: x => x.InvatationId,
                        principalTable: "Invatations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReciverId",
                        column: x => x.ReciverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ACT" },
                    { 2, "NSW" },
                    { 3, "NT" },
                    { 4, "QLD" },
                    { 5, "SA" },
                    { 6, "TAS" },
                    { 7, "VIC" },
                    { 8, "WA" },
                    { 9, "JBT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCvs_EmployeeId",
                table: "EmployeeCvs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCvs_FieldId",
                table: "EmployeeCvs",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExternalMeterials_EmployeeCvId",
                table: "EmployeeExternalMeterials",
                column: "EmployeeCvId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeJobHistories_EmployeeCvId",
                table: "EmployeeJobHistories",
                column: "EmployeeCvId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SchoolId",
                table: "Employees",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SuburbId",
                table: "Employees",
                column: "SuburbId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkills_SkillId",
                table: "EmployeeSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkDays_EmployeeCvId",
                table: "EmployeeWorkDays",
                column: "EmployeeCvId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerComplusoryWorkDays_EmployerJobProfileId",
                table: "EmployerComplusoryWorkDays",
                column: "EmployerJobProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerJobProfiles_EmployerId",
                table: "EmployerJobProfiles",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerJobProfiles_FieldId",
                table: "EmployerJobProfiles",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerJobProfiles_SuburbId",
                table: "EmployerJobProfiles",
                column: "SuburbId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerRequiredSchools_SchoolId",
                table: "EmployerRequiredSchools",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_CertificatedBy",
                table: "Employers",
                column: "CertificatedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employers_SuburbId",
                table: "Employers",
                column: "SuburbId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerSkills_SkillId",
                table: "EmployerSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerWorkLocations_SuburbId",
                table: "EmployerWorkLocations",
                column: "SuburbId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_AddedBy",
                table: "Fields",
                column: "AddedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_NormalizedName",
                table: "Fields",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_RequestedBy",
                table: "Fields",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Invatations_EmployerJobProfileId",
                table: "Invatations",
                column: "EmployerJobProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Invatations_EmployeeCvId_EmployerJobProfileId",
                table: "Invatations",
                columns: new[] { "EmployeeCvId", "EmployerJobProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_InvatationId",
                table: "Messages",
                column: "InvatationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReciverId",
                table: "Messages",
                column: "ReciverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_AddedBy",
                table: "Schools",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_RequestedBy",
                table: "Schools",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SuburbId",
                table: "Schools",
                column: "SuburbId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_DomainExtension_NormalizedName_SuburbId",
                table: "Schools",
                columns: new[] { "DomainExtension", "NormalizedName", "SuburbId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AddedBy",
                table: "Skills",
                column: "AddedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_NormalizedName",
                table: "Skills",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_RequestedByUserId",
                table: "Skills",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suburbs_StateId",
                table: "Suburbs",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmployeeExternalMeterials");

            migrationBuilder.DropTable(
                name: "EmployeeJobHistories");

            migrationBuilder.DropTable(
                name: "EmployeeSkills");

            migrationBuilder.DropTable(
                name: "EmployeeWorkDays");

            migrationBuilder.DropTable(
                name: "EmployerComplusoryWorkDays");

            migrationBuilder.DropTable(
                name: "EmployerRequiredSchools");

            migrationBuilder.DropTable(
                name: "EmployerSkills");

            migrationBuilder.DropTable(
                name: "EmployerWorkLocations");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Invatations");

            migrationBuilder.DropTable(
                name: "EmployeeCvs");

            migrationBuilder.DropTable(
                name: "EmployerJobProfiles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Administraotrs");

            migrationBuilder.DropTable(
                name: "Suburbs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
