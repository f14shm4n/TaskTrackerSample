using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Worker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadAssignemtId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_assignments_work_assignments_HeadAssignemtId",
                        column: x => x.HeadAssignemtId,
                        principalTable: "work_assignments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "work_assignments_relationships",
                columns: table => new
                {
                    Relation = table.Column<int>(type: "int", nullable: false),
                    SourceWorkAssignmentId = table.Column<int>(type: "int", nullable: false),
                    TargetWorkAssignmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_assignments_relationships", x => new { x.Relation, x.SourceWorkAssignmentId, x.TargetWorkAssignmentId });
                    table.ForeignKey(
                        name: "FK_work_assignments_relationships_work_assignments_SourceWorkAssignmentId",
                        column: x => x.SourceWorkAssignmentId,
                        principalTable: "work_assignments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_work_assignments_relationships_work_assignments_TargetWorkAssignmentId",
                        column: x => x.TargetWorkAssignmentId,
                        principalTable: "work_assignments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_work_assignments_HeadAssignemtId",
                table: "work_assignments",
                column: "HeadAssignemtId");

            migrationBuilder.CreateIndex(
                name: "IX_work_assignments_relationships_Relation_SourceWorkAssignmentId_TargetWorkAssignmentId",
                table: "work_assignments_relationships",
                columns: new[] { "Relation", "SourceWorkAssignmentId", "TargetWorkAssignmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_work_assignments_relationships_SourceWorkAssignmentId",
                table: "work_assignments_relationships",
                column: "SourceWorkAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_work_assignments_relationships_TargetWorkAssignmentId",
                table: "work_assignments_relationships",
                column: "TargetWorkAssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "work_assignments_relationships");

            migrationBuilder.DropTable(
                name: "work_assignments");
        }
    }
}
