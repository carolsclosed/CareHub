using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class Doutores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Doutores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistritoProfissional",
                table: "Doutores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "Doutores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "nCedula",
                table: "Doutores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f7c3192-79e5-44a4-be67-1e75a76b84e1", "AQAAAAIAAYagAAAAEPRhSFrwCU7o7o/Vom2+GFP/QqBGPNufT6db+2TSbb7Vvg5qRJsx4dFC8TDw/I7bSg==", "576e0739-e3d1-4151-a60f-642823c0e6e9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Doutores");

            migrationBuilder.DropColumn(
                name: "DistritoProfissional",
                table: "Doutores");

            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Doutores");

            migrationBuilder.DropColumn(
                name: "nCedula",
                table: "Doutores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "84843ded-3a28-4f8c-a9da-cc60bbfc723e", "AQAAAAIAAYagAAAAEOkJXMTCpAGjdxG6SnSDpdNIG4gRnukwt/eVumje4npJL5uv1lHi7VIHiNbDG3ClGw==", "fc7d3220-a981-421c-8302-d3501f86d71b" });
        }
    }
}
