using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Utilizadores",
                type: "nvarchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0dd18589-eb10-444a-87c3-5ec3325abe68", "AQAAAAIAAYagAAAAELGod4EseBu61LEyWUo9srxdlcY9q7PNaeXH29SI2rNRvZ0VpaMBRlcZKjFn2foyDw==", "ecdeb7c6-9d24-41a6-b3d4-6402cab00442" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Utilizadores",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1c30dd45-70a6-41f2-ae95-f0c318de4caa", "AQAAAAIAAYagAAAAEC+M7npX4EYD+FjYQ8u7uLkpYxEx0mxBaSii0khgOq0tuvN0LDn2qp1y+4RZSHAUeA==", "e32ce9b8-7588-4646-9a11-7853312d9aa3" });
        }
    }
}
