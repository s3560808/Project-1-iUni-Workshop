using Microsoft.EntityFrameworkCore.Migrations;

namespace iUniWorkshop.Migrations
{
    public partial class Initiadl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administraotrs",
                columns: table => new
                {
                    AdministratorId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administraotrs", x => x.AdministratorId);
                    table.ForeignKey(
                        name: "FK_Administraotrs_AspNetUsers_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administraotrs");
        }
    }
}
