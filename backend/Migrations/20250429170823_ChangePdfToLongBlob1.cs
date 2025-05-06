using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangePdfToLongBlob1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "FeedbackPDF",
                table: "Feedbacks",
                type: "LONGBLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "FeedbackPDF",
                table: "Feedbacks",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "LONGBLOB",
                oldNullable: true);
        }
    }
}
