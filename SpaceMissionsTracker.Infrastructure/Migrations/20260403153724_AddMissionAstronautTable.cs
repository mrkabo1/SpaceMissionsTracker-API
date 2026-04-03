using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceMissionsTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionAstronautTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MissionAstronauts",
                columns: table => new
                {
                    MissionId = table.Column<int>(type: "int", nullable: false),
                    AstronautId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionAstronauts", x => new { x.MissionId, x.AstronautId });
                    table.ForeignKey(
                        name: "FK_MissionAstronauts_Astronauts_AstronautId",
                        column: x => x.AstronautId,
                        principalTable: "Astronauts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionAstronauts_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MissionAstronauts",
                columns: new[] { "AstronautId", "MissionId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionAstronauts_AstronautId",
                table: "MissionAstronauts",
                column: "AstronautId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionAstronauts");
        }
    }
}
