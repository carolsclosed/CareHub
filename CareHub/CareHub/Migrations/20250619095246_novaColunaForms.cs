using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class novaColunaForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "presencial",
                table: "Formularios",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cdde0a72-7b2e-41f5-89d5-9d20235a603a", "AQAAAAIAAYagAAAAECyghcjeZ3d2jh4xLqw43dQp9XyQngzt5H6HTBPAh0sgDrn0gUWJ57Hzmb24dPaZnw==", "838adf88-601e-4489-8872-1161b9dd69e9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "presencial",
                table: "Formularios");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0687075-7949-4da9-8542-a9dd162c6ba6", "AQAAAAIAAYagAAAAEL3uXZUC7yhrUGiaBaJiVAKJU4Db+HDVlTsXlG+xgncDAPN2ZLNmJHtlSCe8KKhMNQ==", "fa8e68d4-50e8-47a9-9269-7644564f418f" });
        }
    }
}
