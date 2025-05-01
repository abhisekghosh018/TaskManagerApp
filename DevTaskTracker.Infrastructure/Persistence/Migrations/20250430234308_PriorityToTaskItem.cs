using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTaskTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PriorityToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TaskItems",
                newName: "StatusEnum");

            migrationBuilder.AddColumn<int>(
                name: "PriorityEnum",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriorityEnum",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "StatusEnum",
                table: "TaskItems",
                newName: "Status");
        }
    }
}
