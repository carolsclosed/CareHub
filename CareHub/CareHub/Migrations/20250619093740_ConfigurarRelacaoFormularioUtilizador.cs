using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarRelacaoFormularioUtilizador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c09946f4-53dd-4e1d-92cc-b839cbcff485", "AQAAAAIAAYagAAAAED7P1KhBbPsuGV6jHqrJ/Vuw6OLgdlprvKyTnyAuEQiu96l05OO+NxuHxGP0zGeJmQ==", "5efc1f8d-5d48-4b39-bbbd-5f57b8f4860f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6a4b280f-f4f9-4cc0-a93b-59d5d3973788", "AQAAAAIAAYagAAAAED/8iJTlLPX30Xm+4pbFJ6AQ9vRJELyYZkCYmOgFue3NyY0cgwNocoZ98XvUPBtfDg==", "d6915baa-63ce-4e10-b08c-0186440f1242" });
        }
    }
}
