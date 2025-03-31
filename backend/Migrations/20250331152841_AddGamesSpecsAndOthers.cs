using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGamesSpecsAndOthers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LabelsUp = table.Column<string>(type: "TEXT", nullable: false),
                    LabelsRight = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionDown = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionLeft = table.Column<string>(type: "TEXT", nullable: false),
                    Rows = table.Column<int>(type: "INTEGER", nullable: false),
                    Cols = table.Column<int>(type: "INTEGER", nullable: false),
                    BorderColor = table.Column<string>(type: "TEXT", nullable: false),
                    CellColor = table.Column<string>(type: "TEXT", nullable: false),
                    BorderColors = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecisionCosts",
                columns: table => new
                {
                    DecisionCostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DecisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DecisionCostWeight = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionCosts", x => x.DecisionCostId);
                });

            migrationBuilder.CreateTable(
                name: "DecisionEnablers",
                columns: table => new
                {
                    CardEnablerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnablerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionEnablers", x => x.CardEnablerId);
                });

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    DecisionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    DecisionShortDesc = table.Column<string>(type: "TEXT", nullable: false),
                    DecisionLongDesc = table.Column<string>(type: "TEXT", nullable: false),
                    DecisionBaseCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.DecisionId);
                });

            migrationBuilder.CreateTable(
                name: "DecisionWeights",
                columns: table => new
                {
                    DecisionWeightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DecisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightX = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightY = table.Column<int>(type: "INTEGER", nullable: false),
                    SubboardId = table.Column<int>(type: "INTEGER", nullable: false),
                    BoosterX = table.Column<int>(type: "INTEGER", nullable: false),
                    BoosterY = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionWeights", x => x.DecisionWeightId);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DecisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    LongDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                });

            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    GameBoard_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Team_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Game_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Process_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Poz_X = table.Column<int>(type: "INTEGER", nullable: false),
                    Poz_Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Subboard_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoard_id);
                });

            migrationBuilder.CreateTable(
                name: "GameLogMoves",
                columns: table => new
                {
                    GameLogMove_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Board_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Process_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Team_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Move_X = table.Column<int>(type: "INTEGER", nullable: false),
                    Move_Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Sub_board = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLogMoves", x => x.GameLogMove_id);
                });

            migrationBuilder.CreateTable(
                name: "GameLogs",
                columns: table => new
                {
                    GameLog_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Team_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Game_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Decision_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    feedback_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false),
                    Status_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLogs", x => x.GameLog_id);
                });

            migrationBuilder.CreateTable(
                name: "GameLogSpecs",
                columns: table => new
                {
                    GameLogSpec_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameLog_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Process_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Team_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Move_X = table.Column<int>(type: "INTEGER", nullable: false),
                    Move_Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Subboard_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Boost_X = table.Column<int>(type: "INTEGER", nullable: false),
                    Boost_Y = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLogSpecs", x => x.GameLogSpec_id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Game_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Game_desc = table.Column<string>(type: "TEXT", nullable: false),
                    Game_long_desc = table.Column<string>(type: "TEXT", nullable: false),
                    Board_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Game_Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Game_id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Items_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Card_id = table.Column<int>(type: "INTEGER", nullable: false),
                    HardwareShortDesc = table.Column<string>(type: "TEXT", nullable: false),
                    HardwareLongDesc = table.Column<string>(type: "TEXT", nullable: false),
                    ItemsBaseCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Items_id);
                });

            migrationBuilder.CreateTable(
                name: "ItemsCosts",
                columns: table => new
                {
                    ItemsCostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemsCostWeight = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsCosts", x => x.ItemsCostId);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Process_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Process_desc = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessLong_desc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Process_id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Team_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Game_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Team_name = table.Column<string>(type: "TEXT", nullable: false),
                    Team_leader = table.Column<string>(type: "TEXT", nullable: false),
                    Team_bud = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Team_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "DecisionCosts");

            migrationBuilder.DropTable(
                name: "DecisionEnablers");

            migrationBuilder.DropTable(
                name: "Decisions");

            migrationBuilder.DropTable(
                name: "DecisionWeights");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "GameBoards");

            migrationBuilder.DropTable(
                name: "GameLogMoves");

            migrationBuilder.DropTable(
                name: "GameLogs");

            migrationBuilder.DropTable(
                name: "GameLogSpecs");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemsCosts");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
