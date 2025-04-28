using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Migrations
{
    /// <inheritdoc />
    public partial class fix_relations_ship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Projects_ProjectId1",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Statuses_StatusId1",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Boards_ProjectId1",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Boards_StatusId1",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "StatusId1",
                table: "Boards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId1",
                table: "Boards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId1",
                table: "Boards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Boards_ProjectId1",
                table: "Boards",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_StatusId1",
                table: "Boards",
                column: "StatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Projects_ProjectId1",
                table: "Boards",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Statuses_StatusId1",
                table: "Boards",
                column: "StatusId1",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
