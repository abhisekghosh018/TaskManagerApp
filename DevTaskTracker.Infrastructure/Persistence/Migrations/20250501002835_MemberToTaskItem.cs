using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTaskTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MemberToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_AspNetUsers_AssignedToUserId",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "AssignedToUserId",
                table: "TaskItems",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_AssignedToUserId",
                table: "TaskItems",
                newName: "IX_TaskItems_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_AspNetUsers_AppUserId",
                table: "TaskItems",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_AspNetUsers_AppUserId",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "TaskItems",
                newName: "AssignedToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_AppUserId",
                table: "TaskItems",
                newName: "IX_TaskItems_AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_AspNetUsers_AssignedToUserId",
                table: "TaskItems",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
