using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RewardsFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedeemPointsToday",
                table: "VisitorParkInformations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TodayPoints",
                table: "VisitorParkInformations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttractionType",
                table: "VisitedAttractions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialEvent",
                table: "VisitedAttractions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedeemPointsToday",
                table: "VisitorParkInformations");

            migrationBuilder.DropColumn(
                name: "TodayPoints",
                table: "VisitorParkInformations");

            migrationBuilder.DropColumn(
                name: "AttractionType",
                table: "VisitedAttractions");

            migrationBuilder.DropColumn(
                name: "IsSpecialEvent",
                table: "VisitedAttractions");
        }
    }
}
