using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IncidentMaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Incidences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaintenance",
                table: "Incidences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Incidences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Incidences",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "PointsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorParkInformationId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveStrategy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedEntityId = table.Column<int>(type: "int", nullable: true),
                    VisitorParkInformationId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointsHistory_VisitorParkInformations_VisitorParkInformationId",
                        column: x => x.VisitorParkInformationId,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointsHistory_VisitorParkInformations_VisitorParkInformationId1",
                        column: x => x.VisitorParkInformationId1,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointsCost = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    RequiredMembershipLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardRedemptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorParkInformationId = table.Column<int>(type: "int", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: false),
                    RedemptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointsSpent = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RewardId1 = table.Column<int>(type: "int", nullable: true),
                    VisitorParkInformationId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardRedemptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_Rewards_RewardId1",
                        column: x => x.RewardId1,
                        principalTable: "Rewards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_VisitorParkInformations_VisitorParkInformationId",
                        column: x => x.VisitorParkInformationId,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_VisitorParkInformations_VisitorParkInformationId1",
                        column: x => x.VisitorParkInformationId1,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointsHistory_VisitorParkInformationId",
                table: "PointsHistory",
                column: "VisitorParkInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsHistory_VisitorParkInformationId1",
                table: "PointsHistory",
                column: "VisitorParkInformationId1");

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_RewardId",
                table: "RewardRedemptions",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_RewardId1",
                table: "RewardRedemptions",
                column: "RewardId1");

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_VisitorParkInformationId",
                table: "RewardRedemptions",
                column: "VisitorParkInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_VisitorParkInformationId1",
                table: "RewardRedemptions",
                column: "VisitorParkInformationId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointsHistory");

            migrationBuilder.DropTable(
                name: "RewardRedemptions");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "IsMaintenance",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Incidences");
        }
    }
}
