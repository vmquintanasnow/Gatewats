using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Musala.Migrations
{
    public partial class Update_Entity_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uid",
                table: "peripheral");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "peripheral",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "peripheral",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "gateway",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "gateway",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "peripheral");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "peripheral");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "gateway");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "gateway");

            migrationBuilder.AddColumn<int>(
                name: "uid",
                table: "peripheral",
                type: "int",
                nullable: true);
        }
    }
}
