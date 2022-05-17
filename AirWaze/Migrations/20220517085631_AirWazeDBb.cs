using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWaze.Migrations
{
    public partial class AirWazeDBb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Gate_CurrentFlight",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Runway_CurrentFlight",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_CurrentFlight",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Runway");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Gate");

            migrationBuilder.DropColumn(
                name: "Queue",
                table: "Gate");

            migrationBuilder.DropColumn(
                name: "CurrentFlight",
                table: "Flights");

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

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentGateGateID",
                table: "Flights",
                column: "CurrentGateGateID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentRunwayRunwayID",
                table: "Flights",
                column: "CurrentRunwayRunwayID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Gate_CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Runway_CurrentRunwayRunwayID",
                table: "Flights");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Runway",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Gate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Queue",
                table: "Gate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentFlight",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentFlight",
                table: "Flights",
                column: "CurrentFlight",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Gate_CurrentFlight",
                table: "Flights",
                column: "CurrentFlight",
                principalTable: "Gate",
                principalColumn: "GateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Runway_CurrentFlight",
                table: "Flights",
                column: "CurrentFlight",
                principalTable: "Runway",
                principalColumn: "RunwayID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
