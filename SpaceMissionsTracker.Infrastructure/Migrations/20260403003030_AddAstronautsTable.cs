using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceMissionsTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAstronautsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Astronauts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Astronauts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Astronauts",
                columns: new[] { "Id", "BirthYear", "Name", "Nationality" },
                values: new object[,]
                {
                    { 1, 1930, "Neil Armstrong", "USA" },
                    { 2, 1930, "Buzz Aldrin", "USA" },
                    { 3, 1934, "Yuri Gagarin", "USSR" },
                    { 4, 1951, "Sally Ride", "USA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Astronauts");
        }
    }
}
