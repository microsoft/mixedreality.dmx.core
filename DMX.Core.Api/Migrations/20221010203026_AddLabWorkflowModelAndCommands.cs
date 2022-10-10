using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMX.Core.Api.Migrations
{
    public partial class AddLabWorkflowModelAndCommands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabWorkflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    UpdatedBy = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabWorkflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabWorkflowCommands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Arguments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    UpdatedBy = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabWorkflowCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabWorkflowCommands_LabWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "LabWorkflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabWorkflowCommands_WorkflowId",
                table: "LabWorkflowCommands",
                column: "WorkflowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabWorkflowCommands");

            migrationBuilder.DropTable(
                name: "LabWorkflows");
        }
    }
}
