using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class RegistroMelhor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ec018cd-2b07-44c3-b74e-c584648d0e09", "AQAAAAIAAYagAAAAEO3dxxO378w6LP/9JrJXmczlhcE37l86dD73UDkVXdSRLzgPiXUp0Q27unDQ2zZzqA==", "83505213-19a8-46de-af5f-2b2cd9ea43db" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac68bbd5-d393-4187-8ad0-19c697a463d1", "AQAAAAIAAYagAAAAEITpsZSVy4t87du12xDy+FcDtCOKCOLD1qTeHdcrAcCFl7kcSgL/N0LMP/tZASiKOg==", "3e9ce40b-e7f6-4e89-b4aa-4550f9fbc15f" });
        }
    }
}
