using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class StandarizedColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Team_name",
                table: "Teams",
                newName: "TeamToken");

            migrationBuilder.RenameColumn(
                name: "Team_leader",
                table: "Teams",
                newName: "TeamName");

            migrationBuilder.RenameColumn(
                name: "Team_bud",
                table: "Teams",
                newName: "TeamBud");

            migrationBuilder.RenameColumn(
                name: "Team_Token",
                table: "Teams",
                newName: "TeamLeader");

            migrationBuilder.RenameColumn(
                name: "Game_id",
                table: "Teams",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Team_id",
                table: "Teams",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "Process_desc",
                table: "Processes",
                newName: "ProcessLongDesc");

            migrationBuilder.RenameColumn(
                name: "ProcessLong_desc",
                table: "Processes",
                newName: "ProcessDesc");

            migrationBuilder.RenameColumn(
                name: "Process_id",
                table: "Processes",
                newName: "ProcessId");

            migrationBuilder.RenameColumn(
                name: "Card_id",
                table: "Items",
                newName: "CardId");

            migrationBuilder.RenameColumn(
                name: "Items_id",
                table: "Items",
                newName: "ItemsId");

            migrationBuilder.RenameColumn(
                name: "Game_long_desc",
                table: "Games",
                newName: "GameLongDesc");

            migrationBuilder.RenameColumn(
                name: "Game_desc",
                table: "Games",
                newName: "GameDesc");

            migrationBuilder.RenameColumn(
                name: "Board_id",
                table: "Games",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Game_id",
                table: "Games",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Team_id",
                table: "GameLogSpecs",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "Subboard_id",
                table: "GameLogSpecs",
                newName: "SubboardId");

            migrationBuilder.RenameColumn(
                name: "Process_id",
                table: "GameLogSpecs",
                newName: "ProcessId");

            migrationBuilder.RenameColumn(
                name: "Move_Y",
                table: "GameLogSpecs",
                newName: "MoveY");

            migrationBuilder.RenameColumn(
                name: "Move_X",
                table: "GameLogSpecs",
                newName: "MoveX");

            migrationBuilder.RenameColumn(
                name: "GameLog_id",
                table: "GameLogSpecs",
                newName: "GameLogId");

            migrationBuilder.RenameColumn(
                name: "Boost_Y",
                table: "GameLogSpecs",
                newName: "BoostY");

            migrationBuilder.RenameColumn(
                name: "Boost_X",
                table: "GameLogSpecs",
                newName: "BoostX");

            migrationBuilder.RenameColumn(
                name: "GameLogSpec_id",
                table: "GameLogSpecs",
                newName: "GameLogSpecId");

            migrationBuilder.RenameColumn(
                name: "feedback_id",
                table: "GameLogs",
                newName: "feedbackId");

            migrationBuilder.RenameColumn(
                name: "Team_id",
                table: "GameLogs",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "Status_id",
                table: "GameLogs",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "Item_id",
                table: "GameLogs",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "Game_id",
                table: "GameLogs",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Decision_id",
                table: "GameLogs",
                newName: "DecisionId");

            migrationBuilder.RenameColumn(
                name: "GameLog_id",
                table: "GameLogs",
                newName: "GameLogId");

            migrationBuilder.RenameColumn(
                name: "Team_id",
                table: "GameLogMoves",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "Sub_board",
                table: "GameLogMoves",
                newName: "SubBoard");

            migrationBuilder.RenameColumn(
                name: "Process_id",
                table: "GameLogMoves",
                newName: "ProcessId");

            migrationBuilder.RenameColumn(
                name: "Move_Y",
                table: "GameLogMoves",
                newName: "MoveY");

            migrationBuilder.RenameColumn(
                name: "Move_X",
                table: "GameLogMoves",
                newName: "MoveX");

            migrationBuilder.RenameColumn(
                name: "Board_id",
                table: "GameLogMoves",
                newName: "BoardId");

            migrationBuilder.RenameColumn(
                name: "GameLogMove_id",
                table: "GameLogMoves",
                newName: "GameLogMoveId");

            migrationBuilder.RenameColumn(
                name: "Team_id",
                table: "GameBoards",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "Subboard_id",
                table: "GameBoards",
                newName: "SubboardId");

            migrationBuilder.RenameColumn(
                name: "Process_id",
                table: "GameBoards",
                newName: "ProcessId");

            migrationBuilder.RenameColumn(
                name: "Poz_Y",
                table: "GameBoards",
                newName: "PozY");

            migrationBuilder.RenameColumn(
                name: "Poz_X",
                table: "GameBoards",
                newName: "PozX");

            migrationBuilder.RenameColumn(
                name: "Game_id",
                table: "GameBoards",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "GameBoard_id",
                table: "GameBoards",
                newName: "GameBoardId");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "TeamToken",
                table: "Teams",
                newName: "Team_name");

            migrationBuilder.RenameColumn(
                name: "TeamName",
                table: "Teams",
                newName: "Team_leader");

            migrationBuilder.RenameColumn(
                name: "TeamLeader",
                table: "Teams",
                newName: "Team_Token");

            migrationBuilder.RenameColumn(
                name: "TeamBud",
                table: "Teams",
                newName: "Team_bud");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Teams",
                newName: "Game_id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Teams",
                newName: "Team_id");

            migrationBuilder.RenameColumn(
                name: "ProcessLongDesc",
                table: "Processes",
                newName: "Process_desc");

            migrationBuilder.RenameColumn(
                name: "ProcessDesc",
                table: "Processes",
                newName: "ProcessLong_desc");

            migrationBuilder.RenameColumn(
                name: "ProcessId",
                table: "Processes",
                newName: "Process_id");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "Items",
                newName: "Card_id");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "Items",
                newName: "Items_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Games",
                newName: "Board_id");

            migrationBuilder.RenameColumn(
                name: "GameLongDesc",
                table: "Games",
                newName: "Game_long_desc");

            migrationBuilder.RenameColumn(
                name: "GameDesc",
                table: "Games",
                newName: "Game_desc");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Games",
                newName: "Game_id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "GameLogSpecs",
                newName: "Team_id");

            migrationBuilder.RenameColumn(
                name: "SubboardId",
                table: "GameLogSpecs",
                newName: "Subboard_id");

            migrationBuilder.RenameColumn(
                name: "ProcessId",
                table: "GameLogSpecs",
                newName: "Process_id");

            migrationBuilder.RenameColumn(
                name: "MoveY",
                table: "GameLogSpecs",
                newName: "Move_Y");

            migrationBuilder.RenameColumn(
                name: "MoveX",
                table: "GameLogSpecs",
                newName: "Move_X");

            migrationBuilder.RenameColumn(
                name: "GameLogId",
                table: "GameLogSpecs",
                newName: "GameLog_id");

            migrationBuilder.RenameColumn(
                name: "BoostY",
                table: "GameLogSpecs",
                newName: "Boost_Y");

            migrationBuilder.RenameColumn(
                name: "BoostX",
                table: "GameLogSpecs",
                newName: "Boost_X");

            migrationBuilder.RenameColumn(
                name: "GameLogSpecId",
                table: "GameLogSpecs",
                newName: "GameLogSpec_id");

            migrationBuilder.RenameColumn(
                name: "feedbackId",
                table: "GameLogs",
                newName: "feedback_id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "GameLogs",
                newName: "Team_id");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "GameLogs",
                newName: "Status_id");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "GameLogs",
                newName: "Item_id");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameLogs",
                newName: "Game_id");

            migrationBuilder.RenameColumn(
                name: "DecisionId",
                table: "GameLogs",
                newName: "Decision_id");

            migrationBuilder.RenameColumn(
                name: "GameLogId",
                table: "GameLogs",
                newName: "GameLog_id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "GameLogMoves",
                newName: "Team_id");

            migrationBuilder.RenameColumn(
                name: "SubBoard",
                table: "GameLogMoves",
                newName: "Sub_board");

            migrationBuilder.RenameColumn(
                name: "ProcessId",
                table: "GameLogMoves",
                newName: "Process_id");

            migrationBuilder.RenameColumn(
                name: "MoveY",
                table: "GameLogMoves",
                newName: "Move_Y");

            migrationBuilder.RenameColumn(
                name: "MoveX",
                table: "GameLogMoves",
                newName: "Move_X");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "GameLogMoves",
                newName: "Board_id");

            migrationBuilder.RenameColumn(
                name: "GameLogMoveId",
                table: "GameLogMoves",
                newName: "GameLogMove_id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "GameBoards",
                newName: "Team_id");

            migrationBuilder.RenameColumn(
                name: "SubboardId",
                table: "GameBoards",
                newName: "Subboard_id");

            migrationBuilder.RenameColumn(
                name: "ProcessId",
                table: "GameBoards",
                newName: "Process_id");

            migrationBuilder.RenameColumn(
                name: "PozY",
                table: "GameBoards",
                newName: "Poz_Y");

            migrationBuilder.RenameColumn(
                name: "PozX",
                table: "GameBoards",
                newName: "Poz_X");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameBoards",
                newName: "Game_id");

            migrationBuilder.RenameColumn(
                name: "GameBoardId",
                table: "GameBoards",
                newName: "GameBoard_id");
        }
    }
}
