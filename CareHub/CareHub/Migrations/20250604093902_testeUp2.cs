using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class testeUp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtilizadoresIdUtil",
                table: "Comentarios",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa1d6d4c-2949-4f9b-be58-ae63a4bcd5a7", "AQAAAAIAAYagAAAAEKT/keCXJA4zZTTZm8SGMdtLLOmh9jB7v5+vQAzycDeWOYrPmPGd1ecb4PEHOIQtzg==", "252ab27c-8f93-4c9e-8e8f-e5bf0d080a97" });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UtilizadoresIdUtil",
                table: "Comentarios",
                column: "UtilizadoresIdUtil");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Utilizadores_UtilizadoresIdUtil",
                table: "Comentarios",
                column: "UtilizadoresIdUtil",
                principalTable: "Utilizadores",
                principalColumn: "IdUtil");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Utilizadores_UtilizadoresIdUtil",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_UtilizadoresIdUtil",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "UtilizadoresIdUtil",
                table: "Comentarios");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33923791-6ef6-4f1a-843c-b34551b892b8", "AQAAAAIAAYagAAAAEJ0aORmoztZV/eYFiArDQCEGZmg1QztnFZFf2vLz4KfCk+zcHsnfBXZMMWMLzBdG9g==", "1302e718-7b09-40d9-bf20-8f96c5c6531d" });
        }
    }
}
