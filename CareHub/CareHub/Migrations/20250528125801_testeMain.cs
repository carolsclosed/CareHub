using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeMain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "09d888a6-44e7-4c9c-941d-d80bb9bf2f73", "AQAAAAIAAYagAAAAEODk9QIlJ38mh0JzIR2v7pnqG3EdUGYiG8VhSWJZBpPyy/BXpPCthC0Thk4pNReDoQ==", "c77be8ec-ba56-4611-b3fa-ac8202502d29" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "39244fac-1220-48a0-a72b-15d32a50511d", "AQAAAAIAAYagAAAAEDh+ImzU/BhcKcJcTyr+SRAJiOaYndxfyxXNWZPPeBzAW9nuV2cf9CN87bUq8m6uag==", "33c47355-3e95-415c-919b-3901d645c0e6" });
        }
    }
}
