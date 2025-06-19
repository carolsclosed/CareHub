using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class formularios : Migration
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

            migrationBuilder.CreateTable(
                name: "Formularios",
                columns: table => new
                {
                    IdForm = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdUtil = table.Column<int>(type: "INTEGER", nullable: false),
                    nome = table.Column<string>(type: "TEXT", nullable: true),
                    telefone = table.Column<int>(type: "INTEGER", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    regiao = table.Column<string>(type: "TEXT", nullable: true),
                    descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formularios", x => x.IdForm);
                    table.ForeignKey(
                        name: "FK_Formularios_Utilizadores_IdUtil",
                        column: x => x.IdUtil,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtil",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6a4b280f-f4f9-4cc0-a93b-59d5d3973788", "AQAAAAIAAYagAAAAED/8iJTlLPX30Xm+4pbFJ6AQ9vRJELyYZkCYmOgFue3NyY0cgwNocoZ98XvUPBtfDg==", "d6915baa-63ce-4e10-b08c-0186440f1242" });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_IdUtil",
                table: "Comentarios",
                column: "IdUtil");

            migrationBuilder.CreateIndex(
                name: "IX_Formularios_IdUtil",
                table: "Formularios",
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

            migrationBuilder.DropTable(
                name: "Formularios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_IdUtil",
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
