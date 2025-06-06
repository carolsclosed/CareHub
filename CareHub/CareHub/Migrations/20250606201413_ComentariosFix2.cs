using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4be0bc53-ac97-4a88-9578-0598381a474d", "AQAAAAIAAYagAAAAEEinrDmRX96H3gvHOc/1T/cDlT4u40AP/15l8A/nMpcEuTaEvfMjjBTw+gmnM1I8Lg==", "e18c498b-4faf-4f27-999b-2613b60613e5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "45af0baf-546b-46bf-852c-a0836a04936a", "AQAAAAIAAYagAAAAENcpFcBI67j7+/xnUjIQrtBOqXaLdWfNXZ7KKrPPi73o7UsNIoP5mmanUsjLxs2BIA==", "e92d4ab8-9479-42d3-b0ca-92f5fa77ec8e" });
        }
    }
}
