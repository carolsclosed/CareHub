using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeMainUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d11209c-3ff4-4c6d-8b4e-ca0b6fad1bfd", "AQAAAAIAAYagAAAAEMhZt+4Eo+sw1eQvWiNQjxt8JG9yD6MAwmvhsCkXIGqKlxLxwtz8khcQnVcWKkxsKQ==", "3517be1d-bf6b-428c-a181-71ad4d5ae6c7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "09d888a6-44e7-4c9c-941d-d80bb9bf2f73", "AQAAAAIAAYagAAAAEODk9QIlJ38mh0JzIR2v7pnqG3EdUGYiG8VhSWJZBpPyy/BXpPCthC0Thk4pNReDoQ==", "c77be8ec-ba56-4611-b3fa-ac8202502d29" });
        }
    }
}
