using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a49f2693-0f22-4570-ae81-9b1f61349ba1", "AQAAAAIAAYagAAAAEEB/4DMdIEWxvyjk9E6hZCNbqhz03uP0sxeuKeJpz+a78UAeEdhGTLMBeriHtyZ3iA==", "d3191c85-2b21-4f63-93a6-1a92e2c21885" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f78344f-beea-4cd5-a814-dbd01cba84f6", "AQAAAAIAAYagAAAAEA09ZOdLO9e9VAPAdy95smR87/tC8bHv+Ec5c6kUslrZItsGdJ/Z3rEsK2JVCuuKFw==", "8a452cae-2592-4d79-9cad-36b0861917dd" });
        }
    }
}
