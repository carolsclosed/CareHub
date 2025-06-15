using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "467e380b-63de-40e9-92bf-7f5b73f83425", "AQAAAAIAAYagAAAAENDmBvRl2R9Ow/IRMkzAeCTA6faGM71buTfjOEWyTsWh6S6IolA1/xdqezvtOpRtrw==", "c4039680-d557-49c3-9131-34429ec0bde3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1dc713f1-bce4-4f35-84f9-a3b37ae77394", "AQAAAAIAAYagAAAAEIj2nXfn6dRU0E8ekdZ3BoBEC1uH+nTPNbeIH1zKSnkuD9S4s0EDJ/rB/HR+707TQQ==", "a76645f7-8cc0-4cd6-953a-460607562a3d" });
        }
    }
}
