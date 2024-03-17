using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BudgetLowerBound = table.Column<decimal>(type: "TEXT", nullable: false),
                    MoneyRoundingRequired = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomUserIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomUserIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomUserIds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsOwnedShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    ShopItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsOwnedShopItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsOwnedShopItems_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomUserIds_RoomId",
                table: "RoomUserIds",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsOwnedShopItems_RoomId",
                table: "RoomsOwnedShopItems",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomUserIds");

            migrationBuilder.DropTable(
                name: "RoomsOwnedShopItems");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
