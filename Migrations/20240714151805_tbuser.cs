using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ADTest.Migrations
{
    /// <inheritdoc />
    public partial class tbuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "LecturerAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19d08dc5-0d94-46eb-ae65-112d035a8dbc", null, "Student", "Student" },
                    { "25cc6540-053f-4bdf-a605-5edad8bed212", null, "Lecturer", "Lecturer" },
                    { "8b3ed7f0-f459-4d45-ba82-b45bc1923f18", null, "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19d08dc5-0d94-46eb-ae65-112d035a8dbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25cc6540-053f-4bdf-a605-5edad8bed212");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b3ed7f0-f459-4d45-ba82-b45bc1923f18");

            migrationBuilder.DropColumn(
                name: "LecturerAddress",
                table: "AspNetUsers");

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
    }
}
