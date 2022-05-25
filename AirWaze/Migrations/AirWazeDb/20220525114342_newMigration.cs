using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWaze.Migrations.AirWazeDb
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineID);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    DestinationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.DestinationID);
                });

            migrationBuilder.CreateTable(
                name: "Gates",
                columns: table => new
                {
                    GateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    CoordsLat = table.Column<double>(type: "float", nullable: false),
                    CoordsLon = table.Column<double>(type: "float", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<int>(type: "int", nullable: false),
                    Bus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    PlaneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaneNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentAirlineAirlineID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerCapacity = table.Column<int>(type: "int", nullable: false),
                    FuelCapacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightRegion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstClassCapacity = table.Column<int>(type: "int", nullable: false),
                    LoadCapacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FuelUsagePerKM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    SeatDiagramPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AirMiles = table.Column<int>(type: "int", nullable: false),
                    FlightHours = table.Column<int>(type: "int", nullable: false),
                    NextMainentance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.PlaneID);
                    table.ForeignKey(
                        name: "FK_Planes_Airlines_CurrentAirlineAirlineID",
                        column: x => x.CurrentAirlineAirlineID,
                        principalTable: "Airlines",
                        principalColumn: "AirlineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPlanePlaneID = table.Column<int>(type: "int", nullable: true),
                    CurrentPlaneConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestinationID = table.Column<int>(type: "int", nullable: false),
                    CurrentGateGateID = table.Column<int>(type: "int", nullable: false),
                    CurrentRunwayRunwayID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SeatDiagram = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightID);
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_DestinationID",
                        column: x => x.DestinationID,
                        principalTable: "Destinations",
                        principalColumn: "DestinationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Gates_CurrentGateGateID",
                        column: x => x.CurrentGateGateID,
                        principalTable: "Gates",
                        principalColumn: "GateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Planes_CurrentPlanePlaneID",
                        column: x => x.CurrentPlanePlaneID,
                        principalTable: "Planes",
                        principalColumn: "PlaneID");
                    table.ForeignKey(
                        name: "FK_Flights_Runways_CurrentRunwayRunwayID",
                        column: x => x.CurrentRunwayRunwayID,
                        principalTable: "Runways",
                        principalColumn: "RunwayID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentFlightFlightID = table.Column<int>(type: "int", nullable: false),
                    CurrentUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FirstClass = table.Column<bool>(type: "bit", nullable: false),
                    Seat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraLuggage = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_Tickets_Flights_CurrentFlightFlightID",
                        column: x => x.CurrentFlightFlightID,
                        principalTable: "Flights",
                        principalColumn: "FlightID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentGateGateID",
                table: "Flights",
                column: "CurrentGateGateID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentPlanePlaneID",
                table: "Flights",
                column: "CurrentPlanePlaneID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_CurrentRunwayRunwayID",
                table: "Flights",
                column: "CurrentRunwayRunwayID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationID",
                table: "Flights",
                column: "DestinationID");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_CurrentAirlineAirlineID",
                table: "Planes",
                column: "CurrentAirlineAirlineID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CurrentFlightFlightID",
                table: "Tickets",
                column: "CurrentFlightFlightID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CurrentUserId",
                table: "Tickets",
                column: "CurrentUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Gates");

            migrationBuilder.DropTable(
                name: "Planes");

            migrationBuilder.DropTable(
                name: "Runways");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
