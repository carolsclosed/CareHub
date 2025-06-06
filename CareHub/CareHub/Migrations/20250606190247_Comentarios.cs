using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class Comentarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataCom",
                table: "Comentarios",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86b16bd7-9bf6-4ddf-b70e-39498085d340", "AQAAAAIAAYagAAAAEPL3QaikHjMwrizzPL6qyhpQYYmhtqxUA4+3BqXHdiHnzMVSxL/Xac9Cv3VPUiOxbQ==", "49c10776-712a-4422-a62b-aa1717c018a1" });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_IdUtil",
                table: "Comentarios",
                column: "IdUtil");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Utilizadores_IdUtil",
                table: "Comentarios",
                column: "IdUtil",
                principalTable: "Utilizadores",
                principalColumn: "IdUtil",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Utilizadores_IdUtil",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_IdUtil",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "DataCom",
                table: "Comentarios");

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
                values: new object[] { "46fd44f8-b1fa-4f51-b4f7-5dc4fd569b32", "AQAAAAIAAYagAAAAECj0JQdevKwfvSbOsXDya1PZSAqtckCE0Zi55LkgYAXBAf2q7o2M0Bnp7QwHPkWu6g==", "d90c35de-2cbc-4771-bc41-5476f70287d7" });

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
    }
}
