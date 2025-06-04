using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeUp3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46fd44f8-b1fa-4f51-b4f7-5dc4fd569b32", "AQAAAAIAAYagAAAAECj0JQdevKwfvSbOsXDya1PZSAqtckCE0Zi55LkgYAXBAf2q7o2M0Bnp7QwHPkWu6g==", "d90c35de-2cbc-4771-bc41-5476f70287d7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa1d6d4c-2949-4f9b-be58-ae63a4bcd5a7", "AQAAAAIAAYagAAAAEKT/keCXJA4zZTTZm8SGMdtLLOmh9jB7v5+vQAzycDeWOYrPmPGd1ecb4PEHOIQtzg==", "252ab27c-8f93-4c9e-8e8f-e5bf0d080a97" });
        }
    }
}
