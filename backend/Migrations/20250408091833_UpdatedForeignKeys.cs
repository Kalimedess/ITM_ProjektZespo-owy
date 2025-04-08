using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameLogs",
                table: "GameLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameBoards",
                table: "GameBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Processes",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "GameLogId",
                table: "GameLogs");

            migrationBuilder.DropColumn(
                name: "GameBoardId",
                table: "GameBoards");

            migrationBuilder.RenameTable(
                name: "Processes",
                newName: "GameProcess");

            migrationBuilder.RenameColumn(
                name: "DecisionDeck",
                table: "Games",
                newName: "DeckId");

            migrationBuilder.RenameColumn(
                name: "feedbackId",
                table: "GameLogs",
                newName: "FeedbackId");

            migrationBuilder.RenameColumn(
                name: "DecisionDeck",
                table: "Decisions",
                newName: "DeckId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationToken",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TeamToken",
                table: "Teams",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TeamName",
                table: "Teams",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TeamLeader",
                table: "Teams",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HardwareShortDesc",
                table: "Items",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HardwareLongDesc",
                table: "Items",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GameLongDesc",
                table: "Games",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GameDesc",
                table: "Games",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Feedbacks",
                type: "varchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LongDescription",
                table: "Feedbacks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DecisionShortDesc",
                table: "Decisions",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DecisionLongDesc",
                table: "Decisions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Boards",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LabelsUp",
                table: "Boards",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LabelsRight",
                table: "Boards",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionLeft",
                table: "Boards",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionDown",
                table: "Boards",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CellColor",
                table: "Boards",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BorderColors",
                table: "Boards",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BorderColor",
                table: "Boards",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessLongDesc",
                table: "GameProcess",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessDesc",
                table: "GameProcess",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameLogs",
                table: "GameLogs",
                columns: new[] { "GameId", "TeamId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameBoards",
                table: "GameBoards",
                columns: new[] { "GameId", "TeamId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameProcess",
                table: "GameProcess",
                column: "ProcessId");

            migrationBuilder.CreateTable(
                name: "Deck",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeckName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.DeckId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GameId",
                table: "Teams",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsCosts_ItemsId",
                table: "ItemsCosts",
                column: "ItemsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_BoardId",
                table: "Games",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_DeckId",
                table: "Games",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogSpecs_GameLogId_TeamId",
                table: "GameLogSpecs",
                columns: new[] { "GameLogId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameLogSpecs_ProcessId",
                table: "GameLogSpecs",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogSpecs_TeamId",
                table: "GameLogSpecs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_DecisionId",
                table: "GameLogs",
                column: "DecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_FeedbackId",
                table: "GameLogs",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_ItemId",
                table: "GameLogs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_TeamId",
                table: "GameLogs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogMoves_BoardId",
                table: "GameLogMoves",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogMoves_ProcessId",
                table: "GameLogMoves",
                column: "ProcessId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameLogMoves_TeamId",
                table: "GameLogMoves",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_GameId",
                table: "GameBoards",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_ProcessId",
                table: "GameBoards",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_TeamId",
                table: "GameBoards",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DecisionId",
                table: "Feedbacks",
                column: "DecisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecisionWeights_DecisionId",
                table: "DecisionWeights",
                column: "DecisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_DeckId",
                table: "Decisions",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Deck_DeckId",
                table: "Decisions",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "DeckId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DecisionWeights_Decisions_DecisionId",
                table: "DecisionWeights",
                column: "DecisionId",
                principalTable: "Decisions",
                principalColumn: "DecisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Decisions_DecisionId",
                table: "Feedbacks",
                column: "DecisionId",
                principalTable: "Decisions",
                principalColumn: "DecisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameBoards_GameProcess_ProcessId",
                table: "GameBoards",
                column: "ProcessId",
                principalTable: "GameProcess",
                principalColumn: "ProcessId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameBoards_Games_GameId",
                table: "GameBoards",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameBoards_Teams_TeamId",
                table: "GameBoards",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogMoves_Boards_BoardId",
                table: "GameLogMoves",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogMoves_GameProcess_ProcessId",
                table: "GameLogMoves",
                column: "ProcessId",
                principalTable: "GameProcess",
                principalColumn: "ProcessId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogMoves_Teams_TeamId",
                table: "GameLogMoves",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogs_Decisions_DecisionId",
                table: "GameLogs",
                column: "DecisionId",
                principalTable: "Decisions",
                principalColumn: "DecisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogs_Feedbacks_FeedbackId",
                table: "GameLogs",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "FeedbackId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogs_Games_GameId",
                table: "GameLogs",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogs_Items_ItemId",
                table: "GameLogs",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogs_Teams_TeamId",
                table: "GameLogs",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogSpecs_GameLogs_GameLogId_TeamId",
                table: "GameLogSpecs",
                columns: new[] { "GameLogId", "TeamId" },
                principalTable: "GameLogs",
                principalColumns: new[] { "GameId", "TeamId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogSpecs_GameProcess_ProcessId",
                table: "GameLogSpecs",
                column: "ProcessId",
                principalTable: "GameProcess",
                principalColumn: "ProcessId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLogSpecs_Teams_TeamId",
                table: "GameLogSpecs",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Boards_BoardId",
                table: "Games",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Deck_DeckId",
                table: "Games",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "DeckId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_UserId",
                table: "Games",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsCosts_Items_ItemsId",
                table: "ItemsCosts",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "ItemsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Games_GameId",
                table: "Teams",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Deck_DeckId",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_DecisionWeights_Decisions_DecisionId",
                table: "DecisionWeights");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Decisions_DecisionId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_GameBoards_GameProcess_ProcessId",
                table: "GameBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_GameBoards_Games_GameId",
                table: "GameBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_GameBoards_Teams_TeamId",
                table: "GameBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogMoves_Boards_BoardId",
                table: "GameLogMoves");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogMoves_GameProcess_ProcessId",
                table: "GameLogMoves");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogMoves_Teams_TeamId",
                table: "GameLogMoves");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogs_Decisions_DecisionId",
                table: "GameLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogs_Feedbacks_FeedbackId",
                table: "GameLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogs_Games_GameId",
                table: "GameLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogs_Items_ItemId",
                table: "GameLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogs_Teams_TeamId",
                table: "GameLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogSpecs_GameLogs_GameLogId_TeamId",
                table: "GameLogSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogSpecs_GameProcess_ProcessId",
                table: "GameLogSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLogSpecs_Teams_TeamId",
                table: "GameLogSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Boards_BoardId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Deck_DeckId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_UserId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsCosts_Items_ItemsId",
                table: "ItemsCosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Games_GameId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Deck");

            migrationBuilder.DropIndex(
                name: "IX_Teams_GameId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_ItemsCosts_ItemsId",
                table: "ItemsCosts");

            migrationBuilder.DropIndex(
                name: "IX_Games_BoardId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_DeckId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_UserId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_GameLogSpecs_GameLogId_TeamId",
                table: "GameLogSpecs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogSpecs_ProcessId",
                table: "GameLogSpecs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogSpecs_TeamId",
                table: "GameLogSpecs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameLogs",
                table: "GameLogs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogs_DecisionId",
                table: "GameLogs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogs_FeedbackId",
                table: "GameLogs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogs_ItemId",
                table: "GameLogs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogs_TeamId",
                table: "GameLogs");

            migrationBuilder.DropIndex(
                name: "IX_GameLogMoves_BoardId",
                table: "GameLogMoves");

            migrationBuilder.DropIndex(
                name: "IX_GameLogMoves_ProcessId",
                table: "GameLogMoves");

            migrationBuilder.DropIndex(
                name: "IX_GameLogMoves_TeamId",
                table: "GameLogMoves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameBoards",
                table: "GameBoards");

            migrationBuilder.DropIndex(
                name: "IX_GameBoards_GameId",
                table: "GameBoards");

            migrationBuilder.DropIndex(
                name: "IX_GameBoards_ProcessId",
                table: "GameBoards");

            migrationBuilder.DropIndex(
                name: "IX_GameBoards_TeamId",
                table: "GameBoards");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_DecisionId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_DecisionWeights_DecisionId",
                table: "DecisionWeights");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_DeckId",
                table: "Decisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameProcess",
                table: "GameProcess");

            migrationBuilder.RenameTable(
                name: "GameProcess",
                newName: "Processes");

            migrationBuilder.RenameColumn(
                name: "DeckId",
                table: "Games",
                newName: "DecisionDeck");

            migrationBuilder.RenameColumn(
                name: "FeedbackId",
                table: "GameLogs",
                newName: "feedbackId");

            migrationBuilder.RenameColumn(
                name: "DeckId",
                table: "Decisions",
                newName: "DecisionDeck");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationToken",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TeamToken",
                table: "Teams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(6)",
                oldMaxLength: 6)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TeamName",
                table: "Teams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TeamLeader",
                table: "Teams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HardwareShortDesc",
                table: "Items",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HardwareLongDesc",
                table: "Items",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GameLongDesc",
                table: "Games",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GameDesc",
                table: "Games",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "GameLogId",
                table: "GameLogs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "GameBoardId",
                table: "GameBoards",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Feedbacks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2)",
                oldMaxLength: 2)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LongDescription",
                table: "Feedbacks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DecisionShortDesc",
                table: "Decisions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DecisionLongDesc",
                table: "Decisions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LabelsUp",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LabelsRight",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionLeft",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionDown",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CellColor",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BorderColors",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BorderColor",
                table: "Boards",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldMaxLength: 7)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessLongDesc",
                table: "Processes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessDesc",
                table: "Processes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameLogs",
                table: "GameLogs",
                column: "GameLogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameBoards",
                table: "GameBoards",
                column: "GameBoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Processes",
                table: "Processes",
                column: "ProcessId");
        }
    }
}
