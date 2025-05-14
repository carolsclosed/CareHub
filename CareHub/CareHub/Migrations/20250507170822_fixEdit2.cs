using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class fixEdit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8350183b-997a-4b36-9f64-c493d7d17e7b", "AQAAAAIAAYagAAAAELIdcOnrRuOOxI2aBv2v8vyJ14IMSpqftIRPH8e0I9ruQE5Ndu6cCkzNulaW8IwN3Q==", "a5846f1e-4d64-4a3c-a425-cd98c973be7c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d29fd7b3-a526-49c4-80c7-bbd6b123bd87", "AQAAAAIAAYagAAAAEGxXn4zPNnV1Fzfo6W0UcQaorppuA5jPale/8ief3lwGruDWtv+eKzsXg8ZKoCnOSg==", "152f4661-cea0-4be7-97b2-adb02222613b" });
        }
    }
}
