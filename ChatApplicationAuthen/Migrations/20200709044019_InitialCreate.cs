using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplicationAuthen.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    LastName = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Username = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Email = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
