using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFix7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "45af0baf-546b-46bf-852c-a0836a04936a", "AQAAAAIAAYagAAAAENcpFcBI67j7+/xnUjIQrtBOqXaLdWfNXZ7KKrPPi73o7UsNIoP5mmanUsjLxs2BIA==", "e92d4ab8-9479-42d3-b0ca-92f5fa77ec8e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86b16bd7-9bf6-4ddf-b70e-39498085d340", "AQAAAAIAAYagAAAAEPL3QaikHjMwrizzPL6qyhpQYYmhtqxUA4+3BqXHdiHnzMVSxL/Xac9Cv3VPUiOxbQ==", "49c10776-712a-4422-a62b-aa1717c018a1" });
        }
    }
}
