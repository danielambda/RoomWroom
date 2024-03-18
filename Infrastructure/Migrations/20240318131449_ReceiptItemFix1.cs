using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptItemFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceiptItems",
                table: "ReceiptItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceiptItems",
                table: "ReceiptItems",
                columns: new[] { "Id", "ReceiptId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceiptItems",
                table: "ReceiptItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceiptItems",
                table: "ReceiptItems",
                column: "Id");
        }
    }
}
