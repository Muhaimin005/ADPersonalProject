using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ADTest.Migrations
{
    /// <inheritdoc />
    public partial class updateProposalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "111a06fc-b419-434d-83e9-03a0ef2de792");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1db1f287-ea00-4e53-a877-59f56185586c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24962f0b-e000-4d57-b705-33a9d458b51f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "963c00fe-1296-4c32-bcb0-3072960bac45");

            migrationBuilder.AddColumn<byte[]>(
                name: "proposalForm",
                table: "proposal",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "proposalForm",
                table: "proposal");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "111a06fc-b419-434d-83e9-03a0ef2de792", null, "Committee", "Committee" },
                    { "1db1f287-ea00-4e53-a877-59f56185586c", null, "Lecturer", "Lecturer" },
                    { "24962f0b-e000-4d57-b705-33a9d458b51f", null, "Admin", "Admin" },
                    { "963c00fe-1296-4c32-bcb0-3072960bac45", null, "Student", "Student" }
                });
        }
    }
}
