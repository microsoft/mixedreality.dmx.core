using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMX.Core.Api.Migrations
{
    public partial class AddLabDevicesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lab",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerLevel = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    LabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabDevices_Lab_LabId",
                        column: x => x.LabId,
                        principalTable: "Lab",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabDevices_LabId",
                table: "LabDevices",
                column: "LabId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabDevices");

            migrationBuilder.DropTable(
                name: "Lab");
        }
    }
}
