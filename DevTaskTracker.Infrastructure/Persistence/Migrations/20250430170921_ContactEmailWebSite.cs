using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTaskTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactEmailWebSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "Organization");
        }
    }
}
