using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardType = table.Column<string>(type: "ENUM('Decision', 'Software', 'Hardware')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LinkToken = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LicensesOwned = table.Column<int>(type: "int", nullable: false),
                    LicensesUsed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    BoardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LabelsUp = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LabelsRight = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescriptionDown = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescriptionLeft = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    Cols = table.Column<int>(type: "int", nullable: false),
                    BorderColor = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CellColor = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BorderColors = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.BoardId);
                    table.ForeignKey(
                        name: "FK_Boards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeckName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.DeckId);
                    table.ForeignKey(
                        name: "FK_Decks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameEvents",
                columns: table => new
                {
                    GameEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventShortDesc = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventLongDesc = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TurnTime = table.Column<int>(type: "int", nullable: false),
                    DecisionCostWeight = table.Column<double>(type: "double", nullable: true),
                    ItemsCostWeight = table.Column<double>(type: "double", nullable: true),
                    BoosterX = table.Column<double>(type: "double", nullable: true),
                    BoosterY = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvents", x => x.GameEventId);
                    table.ForeignKey(
                        name: "FK_GameEvents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    DecisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    DecisionShortDesc = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DecisionLongDesc = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    DecisionBaseCost = table.Column<double>(type: "double", nullable: false),
                    DecisionCostWeight = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.DecisionId);
                    table.ForeignKey(
                        name: "FK_Decisions_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decisions_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LongDescription = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FeedbackPDF = table.Column<byte[]>(type: "LONGBLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameDesc = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamBoardId = table.Column<int>(type: "int", nullable: false),
                    RivalBoardId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameStatus = table.Column<string>(type: "ENUM('During', 'Paused', 'End')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsOnline = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Boards_RivalBoardId",
                        column: x => x.RivalBoardId,
                        principalTable: "Boards",
                        principalColumn: "BoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Boards_TeamBoardId",
                        column: x => x.TeamBoardId,
                        principalTable: "Boards",
                        principalColumn: "BoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    HardwareShortDesc = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HardwareLongDesc = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemsBaseCost = table.Column<double>(type: "double", nullable: false),
                    ItemsCostWeight = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemsId);
                    table.ForeignKey(
                        name: "FK_Items_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    ProcessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcessDesc = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessLongDesc = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessColor = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessWeight = table.Column<double>(type: "double", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.ProcessId);
                    table.ForeignKey(
                        name: "FK_Processes_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TeamColor = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamBud = table.Column<int>(type: "int", nullable: false),
                    TeamToken = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GameEventId = table.Column<int>(type: "int", nullable: true),
                    TurnsLeft = table.Column<int>(type: "int", nullable: true),
                    IsIndependent = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_GameEvents_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvents",
                        principalColumn: "GameEventId");
                    table.ForeignKey(
                        name: "FK_Teams_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DecisionWeights",
                columns: table => new
                {
                    DecisionWeightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    WeightX = table.Column<int>(type: "int", nullable: false),
                    WeightY = table.Column<int>(type: "int", nullable: false),
                    BoosterX = table.Column<double>(type: "double", nullable: false),
                    BoosterY = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionWeights", x => x.DecisionWeightId);
                    table.ForeignKey(
                        name: "FK_DecisionWeights_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecisionWeights_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecisionWeights_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "ProcessId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DecisionEnablers",
                columns: table => new
                {
                    DecisionEnablerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    EnablerId = table.Column<int>(type: "int", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionEnablers", x => x.DecisionEnablerId);
                    table.ForeignKey(
                        name: "FK_DecisionEnablers_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecisionEnablers_Cards_EnablerId",
                        column: x => x.EnablerId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                    table.ForeignKey(
                        name: "FK_DecisionEnablers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId");
                    table.ForeignKey(
                        name: "FK_DecisionEnablers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameLogs",
                columns: table => new
                {
                    GameLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameEventId = table.Column<int>(type: "int", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: true),
                    DeckId = table.Column<int>(type: "int", nullable: true),
                    BoardId = table.Column<int>(type: "int", nullable: true),
                    FeedbackId = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<double>(type: "double", nullable: true),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLogs", x => x.GameLogId);
                    table.ForeignKey(
                        name: "FK_GameLogs_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "BoardId");
                    table.ForeignKey(
                        name: "FK_GameLogs_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                    table.ForeignKey(
                        name: "FK_GameLogs_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId");
                    table.ForeignKey(
                        name: "FK_GameLogs_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "FeedbackId");
                    table.ForeignKey(
                        name: "FK_GameLogs_GameEvents_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvents",
                        principalColumn: "GameEventId");
                    table.ForeignKey(
                        name: "FK_GameLogs_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLogs_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameProcesses",
                columns: table => new
                {
                    GameProcessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProcesses", x => x.GameProcessId);
                    table.ForeignKey(
                        name: "FK_GameProcesses_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameProcesses_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "ProcessId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameProcesses_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    GameBoardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameProcessId = table.Column<int>(type: "int", nullable: true),
                    PozX = table.Column<double>(type: "double", nullable: false),
                    PozY = table.Column<double>(type: "double", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoardId);
                    table.ForeignKey(
                        name: "FK_GameBoards_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "BoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameBoards_GameProcesses_GameProcessId",
                        column: x => x.GameProcessId,
                        principalTable: "GameProcesses",
                        principalColumn: "GameProcessId");
                    table.ForeignKey(
                        name: "FK_GameBoards_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameBoards_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameLogSpecs",
                columns: table => new
                {
                    GameLogSpecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameLogId = table.Column<int>(type: "int", nullable: false),
                    GameProcessId = table.Column<int>(type: "int", nullable: true),
                    MoveX = table.Column<int>(type: "int", nullable: false),
                    MoveY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLogSpecs", x => x.GameLogSpecId);
                    table.ForeignKey(
                        name: "FK_GameLogSpecs_GameLogs_GameLogId",
                        column: x => x.GameLogId,
                        principalTable: "GameLogs",
                        principalColumn: "GameLogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLogSpecs_GameProcesses_GameProcessId",
                        column: x => x.GameProcessId,
                        principalTable: "GameProcesses",
                        principalColumn: "GameProcessId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_UserId",
                table: "Boards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionEnablers_CardId",
                table: "DecisionEnablers",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionEnablers_EnablerId",
                table: "DecisionEnablers",
                column: "EnablerId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionEnablers_GameId",
                table: "DecisionEnablers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionEnablers_TeamId",
                table: "DecisionEnablers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_CardId",
                table: "Decisions",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_DeckId",
                table: "Decisions",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionWeights_CardId",
                table: "DecisionWeights",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionWeights_DeckId",
                table: "DecisionWeights",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionWeights_ProcessId",
                table: "DecisionWeights",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_UserId",
                table: "Decks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CardId",
                table: "Feedbacks",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DeckId",
                table: "Feedbacks",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_BoardId",
                table: "GameBoards",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_GameId",
                table: "GameBoards",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_GameProcessId",
                table: "GameBoards",
                column: "GameProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_TeamId",
                table: "GameBoards",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEvents_UserId",
                table: "GameEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_BoardId",
                table: "GameLogs",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_CardId",
                table: "GameLogs",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_DeckId",
                table: "GameLogs",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_FeedbackId",
                table: "GameLogs",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_GameEventId",
                table: "GameLogs",
                column: "GameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_GameId",
                table: "GameLogs",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogs_TeamId",
                table: "GameLogs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogSpecs_GameLogId",
                table: "GameLogSpecs",
                column: "GameLogId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLogSpecs_GameProcessId",
                table: "GameLogSpecs",
                column: "GameProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProcesses_GameId",
                table: "GameProcesses",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProcesses_ProcessId",
                table: "GameProcesses",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProcesses_TeamId",
                table: "GameProcesses",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_DeckId",
                table: "Games",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_RivalBoardId",
                table: "Games",
                column: "RivalBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamBoardId",
                table: "Games",
                column: "TeamBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CardId",
                table: "Items",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DeckId",
                table: "Items",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_DeckId",
                table: "Processes",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GameEventId",
                table: "Teams",
                column: "GameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GameId",
                table: "Teams",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionEnablers");

            migrationBuilder.DropTable(
                name: "Decisions");

            migrationBuilder.DropTable(
                name: "DecisionWeights");

            migrationBuilder.DropTable(
                name: "GameBoards");

            migrationBuilder.DropTable(
                name: "GameLogSpecs");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "GameLogs");

            migrationBuilder.DropTable(
                name: "GameProcesses");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "GameEvents");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
