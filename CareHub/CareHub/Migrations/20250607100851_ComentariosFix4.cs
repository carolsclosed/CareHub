using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1dc713f1-bce4-4f35-84f9-a3b37ae77394", "AQAAAAIAAYagAAAAEIj2nXfn6dRU0E8ekdZ3BoBEC1uH+nTPNbeIH1zKSnkuD9S4s0EDJ/rB/HR+707TQQ==", "a76645f7-8cc0-4cd6-953a-460607562a3d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1932ae2c-e40b-4912-a4fe-2b34af4dabe3", "AQAAAAIAAYagAAAAEMaTc9WH/oi+PW1nE3zDdaMmj1sLP+lIIA3MYK2OGZtxq1UPyc21G+d6nye9BUKc8Q==", "176357bb-099c-45e6-bea9-c5e46db3c764" });
        }
    }
}
