using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplicationAuthen.Migrations
{
    public partial class Chat : Migration
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
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ContactMobile = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    UserName = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    Password = table.Column<string>(type: "VARCHAR(64)", nullable: true)
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
