using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class FixHeadAssignmentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_work_assignments_work_assignments_HeadAssignemtId",
                table: "work_assignments");

            migrationBuilder.RenameColumn(
                name: "HeadAssignemtId",
                table: "work_assignments",
                newName: "HeadAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_work_assignments_HeadAssignemtId",
                table: "work_assignments",
                newName: "IX_work_assignments_HeadAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_work_assignments_work_assignments_HeadAssignmentId",
                table: "work_assignments",
                column: "HeadAssignmentId",
                principalTable: "work_assignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_work_assignments_work_assignments_HeadAssignmentId",
                table: "work_assignments");

            migrationBuilder.RenameColumn(
                name: "HeadAssignmentId",
                table: "work_assignments",
                newName: "HeadAssignemtId");

            migrationBuilder.RenameIndex(
                name: "IX_work_assignments_HeadAssignmentId",
                table: "work_assignments",
                newName: "IX_work_assignments_HeadAssignemtId");

            migrationBuilder.AddForeignKey(
                name: "FK_work_assignments_work_assignments_HeadAssignemtId",
                table: "work_assignments",
                column: "HeadAssignemtId",
                principalTable: "work_assignments",
                principalColumn: "Id");
        }
    }
}
