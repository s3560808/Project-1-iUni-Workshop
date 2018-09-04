using Microsoft.EntityFrameworkCore.Migrations;

namespace iUniWorkshop.Migrations
{
    public partial class InitialCredate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "AddedBy", "DomainExtension", "NewRequest", "NormalizedName", "RequestedBy", "SchoolName", "Status", "SuburbId" },
                values: new object[,]
                {
                    { 1, null, "rmit.edu.au", false, "RMIT", null, "RMIT", 1, 1 },
                    { 2, null, "rmit.edu.au", false, "RMIT", null, "RMIT", 1, 2 },
                    { 3, null, "unimelb.edu.au", false, "MELBOURNE UNIVERSITY", null, "Melbourne University", 1, 3 },
                    { 4, null, "unimelb.edu.au", false, "MELBOURNE UNIVERSITY", null, "Melbourne University", 2, 5 },
                    { 5, null, "unimelb.edu.au", false, "MELBOURNE UNIVERSITY", null, "Melbourne University", 3, 6 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
