using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeForm2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6821379b-7006-4fdc-8001-53e610396f16", "AQAAAAIAAYagAAAAEFRlWaESD4BEiPgZyZydJ06q6ylUWO8uWMzoDaMP/iDwdcUGnebwJAWNYzWHibDtzQ==", "902a649e-69d8-45dc-b62b-bf4874cf100c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0dd18589-eb10-444a-87c3-5ec3325abe68", "AQAAAAIAAYagAAAAELGod4EseBu61LEyWUo9srxdlcY9q7PNaeXH29SI2rNRvZ0VpaMBRlcZKjFn2foyDw==", "ecdeb7c6-9d24-41a6-b3d4-6402cab00442" });
        }
    }
}
