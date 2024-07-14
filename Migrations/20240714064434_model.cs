using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ADTest.Migrations
{
    /// <inheritdoc />
    public partial class model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17482271-b88a-4e21-881d-9d3bd7d459fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "465eee4d-4e9b-4ae0-87c2-75eac4a9bcaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ec8afb2-acb7-4690-963c-286eb1043cfb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20af2247-ac5c-4554-b78a-1a295897b908", null, "Admin", "Admin" },
                    { "bd5a8db1-b7bc-4ee7-8d17-14ef88d5a601", null, "Student", "Student" },
                    { "d00b9036-b8c2-426c-8d40-4a4eb72ee5ac", null, "Lecturer", "Lecturer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20af2247-ac5c-4554-b78a-1a295897b908");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd5a8db1-b7bc-4ee7-8d17-14ef88d5a601");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d00b9036-b8c2-426c-8d40-4a4eb72ee5ac");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17482271-b88a-4e21-881d-9d3bd7d459fb", null, "Student", "Student" },
                    { "465eee4d-4e9b-4ae0-87c2-75eac4a9bcaf", null, "Admin", "Admin" },
                    { "4ec8afb2-acb7-4690-963c-286eb1043cfb", null, "Lecturer", "Lecturer" }
                });
        }
    }
}
