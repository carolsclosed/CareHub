using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class afterComentarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4334c0db-2881-4055-ab4f-f4e556552577", "AQAAAAIAAYagAAAAEBIAEH7id5Iq371uvpVXTtsSP8L+HSQs0qxnGtUxZF0bT2kTrFsTuKD0fLy3EYbjcg==", "be94a28e-1521-4196-8b13-8030dbe5b56a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a49f2693-0f22-4570-ae81-9b1f61349ba1", "AQAAAAIAAYagAAAAEEB/4DMdIEWxvyjk9E6hZCNbqhz03uP0sxeuKeJpz+a78UAeEdhGTLMBeriHtyZ3iA==", "d3191c85-2b21-4f63-93a6-1a92e2c21885" });
        }
    }
}
