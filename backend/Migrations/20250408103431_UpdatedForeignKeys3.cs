using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForeignKeys3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Deck_DeckId",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Deck_DeckId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deck",
                table: "Deck");

            migrationBuilder.RenameTable(
                name: "Deck",
                newName: "Decks");

            migrationBuilder.AlterColumn<double>(
                name: "ItemsBaseCost",
                table: "Items",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decks",
                table: "Decks",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Decks_DeckId",
                table: "Decisions",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "DeckId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Decks_DeckId",
                table: "Games",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "DeckId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Decks_DeckId",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Decks_DeckId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decks",
                table: "Decks");

            migrationBuilder.RenameTable(
                name: "Decks",
                newName: "Deck");

            migrationBuilder.AlterColumn<int>(
                name: "ItemsBaseCost",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deck",
                table: "Deck",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Deck_DeckId",
                table: "Decisions",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "DeckId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Deck_DeckId",
                table: "Games",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "DeckId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
