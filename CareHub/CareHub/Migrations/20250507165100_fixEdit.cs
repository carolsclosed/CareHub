using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class fixEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d29fd7b3-a526-49c4-80c7-bbd6b123bd87", "AQAAAAIAAYagAAAAEGxXn4zPNnV1Fzfo6W0UcQaorppuA5jPale/8ief3lwGruDWtv+eKzsXg8ZKoCnOSg==", "152f4661-cea0-4be7-97b2-adb02222613b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8c47991-19e0-4d2d-854f-974df3f0073f", "AQAAAAIAAYagAAAAEJddmuSBju8P1AVje2FTPVaNnNihkdu6+ovxiHqiOdgIiotipj/WhnudeCz/b2QZ6w==", "8d16bfc8-964e-4436-86de-e5756a3685e0" });
        }
    }
}
