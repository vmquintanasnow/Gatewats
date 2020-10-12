using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Musala.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gateway",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(64)", nullable: false),
                    name = table.Column<string>(type: "varchar(64)", nullable: true),
                    ipv4 = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gateway", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "peripheral",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<int>(nullable: true),
                    vendor = table.Column<string>(type: "varchar(64)", nullable: true),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(nullable: true),
                    gateway_id = table.Column<string>(type: "varchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peripheral", x => x.id);
                    table.ForeignKey(
                        name: "Gateway_Peripheral",
                        column: x => x.gateway_id,
                        principalTable: "gateway",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Gateway_Peripheral",
                table: "peripheral",
                column: "gateway_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "peripheral");

            migrationBuilder.DropTable(
                name: "gateway");
        }
    }
}
