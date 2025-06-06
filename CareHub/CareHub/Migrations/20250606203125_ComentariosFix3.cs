using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1932ae2c-e40b-4912-a4fe-2b34af4dabe3", "AQAAAAIAAYagAAAAEMaTc9WH/oi+PW1nE3zDdaMmj1sLP+lIIA3MYK2OGZtxq1UPyc21G+d6nye9BUKc8Q==", "176357bb-099c-45e6-bea9-c5e46db3c764" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4be0bc53-ac97-4a88-9578-0598381a474d", "AQAAAAIAAYagAAAAEEinrDmRX96H3gvHOc/1T/cDlT4u40AP/15l8A/nMpcEuTaEvfMjjBTw+gmnM1I8Lg==", "e18c498b-4faf-4f27-999b-2613b60613e5" });
        }
    }
}
