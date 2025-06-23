using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "84843ded-3a28-4f8c-a9da-cc60bbfc723e", "AQAAAAIAAYagAAAAEOkJXMTCpAGjdxG6SnSDpdNIG4gRnukwt/eVumje4npJL5uv1lHi7VIHiNbDG3ClGw==", "fc7d3220-a981-421c-8302-d3501f86d71b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b232e1c-9360-4198-b2bc-092c0b8f6ad4", "AQAAAAIAAYagAAAAEC9a9w92W4WZ3ius6pX3UoEUADRC95GepiECewa7jkgjWIrNKYRPF02sZOlu7/oqnQ==", "02c3c0a0-312c-41a3-a1af-7ae8e47ded03" });
        }
    }
}
