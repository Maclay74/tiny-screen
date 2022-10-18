using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyScreen.Migrations
{
    /// <inheritdoc />
    public partial class GameFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Folders_FolderId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_FolderId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "FolderGame",
                columns: table => new
                {
                    FoldersId = table.Column<int>(type: "INTEGER", nullable: false),
                    GamesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderGame", x => new { x.FoldersId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_FolderGame_Folders_FoldersId",
                        column: x => x.FoldersId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderGame_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderGame_GamesId",
                table: "FolderGame",
                column: "GamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderGame");

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "Games",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_FolderId",
                table: "Games",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Folders_FolderId",
                table: "Games",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
