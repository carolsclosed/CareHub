using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class fixAdminFoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Utilizadores",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1c30dd45-70a6-41f2-ae95-f0c318de4caa", "AQAAAAIAAYagAAAAEC+M7npX4EYD+FjYQ8u7uLkpYxEx0mxBaSii0khgOq0tuvN0LDn2qp1y+4RZSHAUeA==", "e32ce9b8-7588-4646-9a11-7853312d9aa3" });

            migrationBuilder.UpdateData(
                table: "Utilizadores",
                keyColumn: "IdUtil",
                keyValue: 1,
                column: "Foto",
                value: "/ImagensUtilizadores/user.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Utilizadores",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b983cc1c-6314-40e1-a2d5-febdd5f4ad09", "AQAAAAIAAYagAAAAEPpYsr6Ad3xHOROwVBek95Hko94p0Uz9TpsKlN7K4wqTZ+o3chA8NPpOsTTVmJGSqw==", "640e385a-25e9-48fa-868a-15ec770caf34" });

            migrationBuilder.UpdateData(
                table: "Utilizadores",
                keyColumn: "IdUtil",
                keyValue: 1,
                column: "Foto",
                value: null);
        }
    }
}
