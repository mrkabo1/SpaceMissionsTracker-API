using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceMissionsTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RocketId = table.Column<int>(type: "int", nullable: false),
                    LaunchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Missions_Rockets_RocketId",
                        column: x => x.RocketId,
                        principalTable: "Rockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Missions",
                columns: new[] { "Id", "Destination", "LaunchDate", "Name", "RocketId", "Status" },
                values: new object[,]
                {
                    { 1, "Moon", new DateTime(1969, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apollo 11", 1, "Completed" },
                    { 2, "Earth Orbit", new DateTime(1961, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vostok 1", 3, "Completed" },
                    { 3, "Moon Orbit", new DateTime(2022, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Artemis I", 1, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_RocketId",
                table: "Missions",
                column: "RocketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Missions");
        }
    }
}
