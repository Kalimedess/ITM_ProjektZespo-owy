using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDecisionIdIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Usunięcie klucza obcego
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Decisions_DecisionId",
                table: "feedbacks");
    
            // Usunięcie indeksu
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_DecisionId",
                table: "feedbacks");
        }
    
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Przywrócenie indeksu
            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DecisionId",
                table: "feedbacks",
                column: "DecisionId");
    
            // Przywrócenie klucza obcego
            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Decisions_DecisionId",
                table: "feedbacks",
                column: "DecisionId",
                principalTable: "decisions",
                principalColumn: "DecisionId",
                onDelete: ReferentialAction.Cascade);
        }
    }

}
