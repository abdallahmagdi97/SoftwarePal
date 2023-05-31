using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftwarePal.Migrations
{
    public partial class addwishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserUpdated",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UserUpdated",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "WishLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishLists", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishLists");

            migrationBuilder.AddColumn<string>(
                name: "UserCreated",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdated",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CartItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdated",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
