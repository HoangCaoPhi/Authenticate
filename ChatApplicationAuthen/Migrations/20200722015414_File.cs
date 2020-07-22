using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplicationAuthen.Migrations
{
    public partial class File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "VARCHAR(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    convId = table.Column<string>(nullable: true),
                    content = table.Column<string>(nullable: true),
                    filePath = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false),
                    typeOf = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldNullable: true);
        }
    }
}
