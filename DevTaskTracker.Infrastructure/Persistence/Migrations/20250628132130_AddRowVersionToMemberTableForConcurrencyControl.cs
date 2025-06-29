﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTaskTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToMemberTableForConcurrencyControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Members",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Members");
        }
    }
}
