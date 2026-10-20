using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixSpecialEvent_Attraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_SpecialEvents_SpecialEventId",
                table: "Attractions");

            migrationBuilder.DropIndex(
                name: "IX_Attractions_SpecialEventId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "SpecialEventId",
                table: "Attractions");

            migrationBuilder.CreateTable(
                name: "specialEvent_Attraction",
                columns: table => new
                {
                    IncludedAttractionsId = table.Column<int>(type: "int", nullable: false),
                    SpecialEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialEvent_Attraction", x => new { x.IncludedAttractionsId, x.SpecialEventId });
                    table.ForeignKey(
                        name: "FK_specialEvent_Attraction_Attractions_IncludedAttractionsId",
                        column: x => x.IncludedAttractionsId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_specialEvent_Attraction_SpecialEvents_SpecialEventId",
                        column: x => x.SpecialEventId,
                        principalTable: "SpecialEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_specialEvent_Attraction_SpecialEventId",
                table: "specialEvent_Attraction",
                column: "SpecialEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "specialEvent_Attraction");

            migrationBuilder.AddColumn<int>(
                name: "SpecialEventId",
                table: "Attractions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_SpecialEventId",
                table: "Attractions",
                column: "SpecialEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_SpecialEvents_SpecialEventId",
                table: "Attractions",
                column: "SpecialEventId",
                principalTable: "SpecialEvents",
                principalColumn: "Id");
        }
    }
}
