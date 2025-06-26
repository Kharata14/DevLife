using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevLife.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCasinoGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CasinoGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BetAmount = table.Column<int>(type: "integer", nullable: false),
                    Payout = table.Column<int>(type: "integer", nullable: false),
                    ChosenSnippetId = table.Column<string>(type: "text", nullable: false),
                    WasCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasinoGames", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CasinoGames");
        }
    }
}
