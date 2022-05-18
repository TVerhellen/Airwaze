using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirWaze.Migrations
{
    public partial class HaraldMigrate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirMiles",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConstructionYear",
                table: "Planes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FlightHours",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NextMainentance",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeatDiagramPic",
                table: "Planes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirMiles",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "ConstructionYear",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "FlightHours",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "NextMainentance",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "SeatDiagramPic",
                table: "Planes");
        }
    }
}
