using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarRelacaoFormularioUtilizador2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Utilizadores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0687075-7949-4da9-8542-a9dd162c6ba6", "AQAAAAIAAYagAAAAEL3uXZUC7yhrUGiaBaJiVAKJU4Db+HDVlTsXlG+xgncDAPN2ZLNmJHtlSCe8KKhMNQ==", "fa8e68d4-50e8-47a9-9269-7644564f418f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Utilizadores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c09946f4-53dd-4e1d-92cc-b839cbcff485", "AQAAAAIAAYagAAAAED7P1KhBbPsuGV6jHqrJ/Vuw6OLgdlprvKyTnyAuEQiu96l05OO+NxuHxGP0zGeJmQ==", "5efc1f8d-5d48-4b39-bbbd-5f57b8f4860f" });
        }
    }
}
