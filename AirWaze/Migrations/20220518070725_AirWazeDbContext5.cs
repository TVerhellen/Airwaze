using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWaze.Migrations
{
    public partial class AirWazeDbContext5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Gates_CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Runways_CurrentRunwayRunwayID",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "Gates");

            migrationBuilder.DropTable(
                name: "Runways");

            migrationBuilder.DropIndex(
                name: "IX_Flights_CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_CurrentRunwayRunwayID",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "CurrentRunwayRunwayID",
                table: "Flights");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentGateGateID",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRunwayRunwayID",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    GateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gates", x => x.GateID);
                });

            migrationBuilder.CreateTable(
                name: "Runways",
                columns: table => new
                {
                    RunwayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runways", x => x.RunwayID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentGateGateID",
                table: "Flights",
                column: "CurrentGateGateID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentRunwayRunwayID",
                table: "Flights",
                column: "CurrentRunwayRunwayID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Gates_CurrentGateGateID",
                table: "Flights",
                column: "CurrentGateGateID",
                principalTable: "Gates",
                principalColumn: "GateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Runways_CurrentRunwayRunwayID",
                table: "Flights",
                column: "CurrentRunwayRunwayID",
                principalTable: "Runways",
                principalColumn: "RunwayID");
        }
    }
}
