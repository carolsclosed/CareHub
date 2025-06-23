using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class Doutoresemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Doutores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f25652fa-867e-4c94-86d5-3ccfd9440941", "AQAAAAIAAYagAAAAEDqdCmxYXHLQBSOxxuqGKDttClwoNA0sFG9N38sMH7pARF2ZyL4b5Ly5fkWyfyMGfQ==", "e6a1c131-c7c7-411c-aea9-254b63d66a8e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Doutores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26032608-40b2-468e-80a5-ab21259f5958", "AQAAAAIAAYagAAAAEB0vvYGCwcp6UzquxmkQMeuezUVh282LgLjwAEVPGkhdjWZiozYxutJSX/W0ExmSmw==", "b8440d29-885d-4689-9477-63430e2e7d8a" });
        }
    }
}
