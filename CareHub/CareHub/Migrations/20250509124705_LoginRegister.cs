using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class LoginRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Utilizadores");

            migrationBuilder.AddColumn<string>(
                name: "IdentityRole",
                table: "Utilizadores",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserName",
                table: "Utilizadores",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac68bbd5-d393-4187-8ad0-19c697a463d1", "AQAAAAIAAYagAAAAEITpsZSVy4t87du12xDy+FcDtCOKCOLD1qTeHdcrAcCFl7kcSgL/N0LMP/tZASiKOg==", "3e9ce40b-e7f6-4e89-b4aa-4550f9fbc15f" });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_IdPost",
                table: "Comentarios",
                column: "IdPost");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Posts_IdPost",
                table: "Comentarios",
                column: "IdPost",
                principalTable: "Posts",
                principalColumn: "IdPost",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Posts_IdPost",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_IdPost",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "IdentityRole",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "IdentityUserName",
                table: "Utilizadores");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Utilizadores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8350183b-997a-4b36-9f64-c493d7d17e7b", "AQAAAAIAAYagAAAAELIdcOnrRuOOxI2aBv2v8vyJ14IMSpqftIRPH8e0I9ruQE5Ndu6cCkzNulaW8IwN3Q==", "a5846f1e-4d64-4a3c-a425-cd98c973be7c" });
        }
    }
}
