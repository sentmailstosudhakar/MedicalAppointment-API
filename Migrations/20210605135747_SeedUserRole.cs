using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAppointment_API.Migrations
{
    public partial class SeedUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Admin" }
                );

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "name" },
                values: new object[] { 2, "Doctor" }
                );

            migrationBuilder.InsertData(
               table: "UserRole",
               columns: new[] { "id", "name" },
               values: new object[] { 3, "Patient" }
               );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "id",
                keyValue: "1"
                );

            migrationBuilder.DeleteData(
               table: "UserRole",
               keyColumn: "id",
               keyValue: "2"
               );

            migrationBuilder.DeleteData(
               table: "UserRole",
               keyColumn: "id",
               keyValue: "3"
               );
        }
    }
}
