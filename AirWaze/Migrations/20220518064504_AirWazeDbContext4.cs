using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWaze.Migrations
{
    public partial class AirWazeDbContext4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Gate_CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Runway_CurrentRunwayRunwayID",
                table: "Flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Runway",
                table: "Runway");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gate",
                table: "Gate");

            migrationBuilder.RenameTable(
                name: "Runway",
                newName: "Runways");

            migrationBuilder.RenameTable(
                name: "Gate",
                newName: "Gates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Runways",
                table: "Runways",
                column: "RunwayID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gates",
                table: "Gates",
                column: "GateID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Gates_CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Runways_CurrentRunwayRunwayID",
                table: "Flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Runways",
                table: "Runways");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gates",
                table: "Gates");

            migrationBuilder.RenameTable(
                name: "Runways",
                newName: "Runway");

            migrationBuilder.RenameTable(
                name: "Gates",
                newName: "Gate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Runway",
                table: "Runway",
                column: "RunwayID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gate",
                table: "Gate",
                column: "GateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Gate_CurrentGateGateID",
                table: "Flights",
                column: "CurrentGateGateID",
                principalTable: "Gate",
                principalColumn: "GateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Runway_CurrentRunwayRunwayID",
                table: "Flights",
                column: "CurrentRunwayRunwayID",
                principalTable: "Runway",
                principalColumn: "RunwayID");
        }
    }
}
