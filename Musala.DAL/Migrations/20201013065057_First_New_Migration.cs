using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Musala.DAL.Migrations
{
    public partial class First_New_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gateway",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(64)", nullable: false),
                    name = table.Column<string>(type: "varchar(64)", nullable: true),
                    ipv4 = table.Column<string>(type: "varchar(15)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gateway", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vendor = table.Column<string>(type: "varchar(64)", nullable: true),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    gateway_id = table.Column<string>(type: "varchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device", x => x.id);
                    table.ForeignKey(
                        name: "Gateway_Device",
                        column: x => x.gateway_id,
                        principalTable: "gateway",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Gateway_Device",
                table: "device",
                column: "gateway_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropTable(
                name: "gateway");
        }
    }
}
