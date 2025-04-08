using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForeignKeys2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DecisionCosts_DecisionId",
                table: "DecisionCosts",
                column: "DecisionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DecisionCosts_Decisions_DecisionId",
                table: "DecisionCosts",
                column: "DecisionId",
                principalTable: "Decisions",
                principalColumn: "DecisionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DecisionCosts_Decisions_DecisionId",
                table: "DecisionCosts");

            migrationBuilder.DropIndex(
                name: "IX_DecisionCosts_DecisionId",
                table: "DecisionCosts");
        }
    }
}
