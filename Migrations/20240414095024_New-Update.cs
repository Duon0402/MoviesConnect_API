using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class NewUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "AspNetUsers",
                newName: "IsPrivate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Ratings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Ratings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "DeletedId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "AspNetUsers",
                newName: "IsPublic");
        }
    }
}
