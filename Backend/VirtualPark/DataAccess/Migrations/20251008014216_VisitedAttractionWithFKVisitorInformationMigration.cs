using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VisitedAttractionWithFKVisitorInformationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int", nullable: false),
                    AdditionalCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TypeAttraction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int", nullable: false),
                    SpecialEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attractions_SpecialEvents_SpecialEventId",
                        column: x => x.SpecialEventId,
                        principalTable: "SpecialEvents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VisitorParkInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    VisitorMembership = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    NFC = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorParkInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorParkInformations_Visitors_Id",
                        column: x => x.Id,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    AttractionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidences_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorParkInformationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QR = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Consumed = table.Column<bool>(type: "bit", nullable: false),
                    SpecialEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_SpecialEvents_SpecialEventId",
                        column: x => x.SpecialEventId,
                        principalTable: "SpecialEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tickets_VisitorParkInformations_VisitorParkInformationId",
                        column: x => x.VisitorParkInformationId,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitedAttractions",
                columns: table => new
                {
                    VisitorParkInformationId = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitorParkInformationId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitedAttractions", x => new { x.VisitorParkInformationId, x.AttractionId, x.VisitDate });
                    table.ForeignKey(
                        name: "FK_VisitedAttractions_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitedAttractions_VisitorParkInformations_VisitorParkInformationId",
                        column: x => x.VisitorParkInformationId,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitedAttractions_VisitorParkInformations_VisitorParkInformationId1",
                        column: x => x.VisitorParkInformationId1,
                        principalTable: "VisitorParkInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_Name",
                table: "Attractions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_SpecialEventId",
                table: "Attractions",
                column: "SpecialEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidences_AttractionId",
                table: "Incidences",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_QR",
                table: "Tickets",
                column: "QR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SpecialEventId",
                table: "Tickets",
                column: "SpecialEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_VisitorParkInformationId",
                table: "Tickets",
                column: "VisitorParkInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedAttractions_AttractionId",
                table: "VisitedAttractions",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedAttractions_VisitorParkInformationId1",
                table: "VisitedAttractions",
                column: "VisitorParkInformationId1");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorParkInformations_NFC",
                table: "VisitorParkInformations",
                column: "NFC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_Email",
                table: "Visitors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Email",
                table: "Workers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidences");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "VisitedAttractions");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Attractions");

            migrationBuilder.DropTable(
                name: "VisitorParkInformations");

            migrationBuilder.DropTable(
                name: "SpecialEvents");

            migrationBuilder.DropTable(
                name: "Visitors");
        }
    }
}
