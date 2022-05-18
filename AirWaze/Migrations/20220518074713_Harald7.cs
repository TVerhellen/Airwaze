using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWaze.Migrations
{
    public partial class Harald7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Runway",
                table: "Runway");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gate",
                table: "Gate");

            migrationBuilder.DropColumn(
                name: "CurrentGateGateID",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "CurrentRunwayRunwayID",
                table: "Flights");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Runway",
                table: "Runway",
                column: "RunwayID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gate",
                table: "Gate",
                column: "GateID");

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
    }
}
