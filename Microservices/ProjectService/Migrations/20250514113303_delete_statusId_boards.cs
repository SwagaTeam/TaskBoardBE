using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Migrations
{
    /// <inheritdoc />
    public partial class delete_statusId_boards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Boards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Boards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
