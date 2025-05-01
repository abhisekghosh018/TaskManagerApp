using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTaskTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PriorityAndMemberToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusEnum",
                table: "TaskItems",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PriorityEnum",
                table: "TaskItems",
                newName: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TaskItems",
                newName: "StatusEnum");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "TaskItems",
                newName: "PriorityEnum");
        }
    }
}
