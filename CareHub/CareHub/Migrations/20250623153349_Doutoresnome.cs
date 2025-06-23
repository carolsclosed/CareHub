using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class Doutoresnome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Doutores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26032608-40b2-468e-80a5-ab21259f5958", "AQAAAAIAAYagAAAAEB0vvYGCwcp6UzquxmkQMeuezUVh282LgLjwAEVPGkhdjWZiozYxutJSX/W0ExmSmw==", "b8440d29-885d-4689-9477-63430e2e7d8a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Doutores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f7c3192-79e5-44a4-be67-1e75a76b84e1", "AQAAAAIAAYagAAAAEPRhSFrwCU7o7o/Vom2+GFP/QqBGPNufT6db+2TSbb7Vvg5qRJsx4dFC8TDw/I7bSg==", "576e0739-e3d1-4151-a60f-642823c0e6e9" });
        }
    }
}
