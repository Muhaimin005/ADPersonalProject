using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ADTest.Migrations
{
    /// <inheritdoc />
    public partial class updateProposalModel01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c7b0002-0560-4aad-8b9b-1844a588ac65");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e0728b1-6d8b-49f0-b010-2ae9839a5514");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5159bd6a-01c4-4489-968a-616cd0a52062");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58ecf32f-4c9d-4306-999c-07487f4ce547");

            migrationBuilder.AddColumn<int>(
                name: "semester",
                table: "proposal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "session",
                table: "proposal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39f2cfc3-d31c-4345-bd55-8ce2b8a62bac", null, "Student", "Student" },
                    { "3f0f83d6-cf92-4c0b-a4f4-453fdbda530c", null, "Committee", "Committee" },
                    { "68cdb623-359c-454c-b9c3-04f1c71a4a33", null, "Admin", "Admin" },
                    { "b69646dd-e06a-4cfb-8fae-93827054a13f", null, "Lecturer", "Lecturer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39f2cfc3-d31c-4345-bd55-8ce2b8a62bac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f0f83d6-cf92-4c0b-a4f4-453fdbda530c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68cdb623-359c-454c-b9c3-04f1c71a4a33");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b69646dd-e06a-4cfb-8fae-93827054a13f");

            migrationBuilder.DropColumn(
                name: "semester",
                table: "proposal");

            migrationBuilder.DropColumn(
                name: "session",
                table: "proposal");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c7b0002-0560-4aad-8b9b-1844a588ac65", null, "Lecturer", "Lecturer" },
                    { "4e0728b1-6d8b-49f0-b010-2ae9839a5514", null, "Committee", "Committee" },
                    { "5159bd6a-01c4-4489-968a-616cd0a52062", null, "Student", "Student" },
                    { "58ecf32f-4c9d-4306-999c-07487f4ce547", null, "Admin", "Admin" }
                });
        }
    }
}
