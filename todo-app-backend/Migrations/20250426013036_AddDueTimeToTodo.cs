using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoappbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDueTimeToTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "DueTime",
                table: "Todos",
                type: "time(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueTime",
                table: "Todos");
        }
    }
}
