using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ImagemPerfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Utilizadores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f4a71587-b9cb-4c00-b051-0beb803bf475", "AQAAAAIAAYagAAAAED7IfPQ07kpuZVT64N4pKoc9TW+DKt9LKGBaD9lra1ULP0op9XBbnZEOtUGbgX7tMA==", "765d419e-3ac0-44c3-8693-f9ed04dbc47a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Utilizadores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ec018cd-2b07-44c3-b74e-c584648d0e09", "AQAAAAIAAYagAAAAEO3dxxO378w6LP/9JrJXmczlhcE37l86dD73UDkVXdSRLzgPiXUp0Q27unDQ2zZzqA==", "83505213-19a8-46de-af5f-2b2cd9ea43db" });
        }
    }
}
