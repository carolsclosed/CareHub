using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33923791-6ef6-4f1a-843c-b34551b892b8", "AQAAAAIAAYagAAAAEJ0aORmoztZV/eYFiArDQCEGZmg1QztnFZFf2vLz4KfCk+zcHsnfBXZMMWMLzBdG9g==", "1302e718-7b09-40d9-bf20-8f96c5c6531d" });

            migrationBuilder.CreateIndex(
                name: "IX_Ups_IdPost",
                table: "Ups",
                column: "IdPost");

            migrationBuilder.AddForeignKey(
                name: "FK_Ups_Posts_IdPost",
                table: "Ups",
                column: "IdPost",
                principalTable: "Posts",
                principalColumn: "IdPost",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ups_Utilizadores_IdUtil",
                table: "Ups",
                column: "IdUtil",
                principalTable: "Utilizadores",
                principalColumn: "IdUtil",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ups_Posts_IdPost",
                table: "Ups");

            migrationBuilder.DropForeignKey(
                name: "FK_Ups_Utilizadores_IdUtil",
                table: "Ups");

            migrationBuilder.DropIndex(
                name: "IX_Ups_IdPost",
                table: "Ups");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f4a71587-b9cb-4c00-b051-0beb803bf475", "AQAAAAIAAYagAAAAED7IfPQ07kpuZVT64N4pKoc9TW+DKt9LKGBaD9lra1ULP0op9XBbnZEOtUGbgX7tMA==", "765d419e-3ac0-44c3-8693-f9ed04dbc47a" });
        }
    }
}
