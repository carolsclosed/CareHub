using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1f78344f-beea-4cd5-a814-dbd01cba84f6", "AQAAAAIAAYagAAAAEA09ZOdLO9e9VAPAdy95smR87/tC8bHv+Ec5c6kUslrZItsGdJ/Z3rEsK2JVCuuKFw==", "8a452cae-2592-4d79-9cad-36b0861917dd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "467e380b-63de-40e9-92bf-7f5b73f83425", "AQAAAAIAAYagAAAAENDmBvRl2R9Ow/IRMkzAeCTA6faGM71buTfjOEWyTsWh6S6IolA1/xdqezvtOpRtrw==", "c4039680-d557-49c3-9131-34429ec0bde3" });
        }
    }
}
