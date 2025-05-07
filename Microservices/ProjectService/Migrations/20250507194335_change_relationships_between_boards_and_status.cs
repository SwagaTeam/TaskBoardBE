using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Migrations
{
    /// <inheritdoc />
    public partial class change_relationships_between_boards_and_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Statuses_StatusId",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Boards_StatusId",
                table: "Boards");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "UserEntity",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Statuses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ItemsBoards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_BoardId",
                table: "Statuses",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsBoards_StatusId",
                table: "ItemsBoards",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsBoards_Statuses_StatusId",
                table: "ItemsBoards",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statuses_Boards_BoardId",
                table: "Statuses",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsBoards_Statuses_StatusId",
                table: "ItemsBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_Statuses_Boards_BoardId",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_BoardId",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_ItemsBoards_StatusId",
                table: "ItemsBoards");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "UserEntity");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ItemsBoards");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_StatusId",
                table: "Boards",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Statuses_StatusId",
                table: "Boards",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
