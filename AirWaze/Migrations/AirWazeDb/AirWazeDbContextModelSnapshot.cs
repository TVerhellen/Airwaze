// <auto-generated />
using System;
using AirWaze.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirWaze.Migrations.AirWazeDb
{
    [DbContext(typeof(AirWazeDbContext))]
    partial class AirWazeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AirWaze.Entities.Airline", b =>
                {
                    b.Property<Guid>("AirlineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdminComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameTag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AirlineID");

                    b.ToTable("Airlines");
                });

            modelBuilder.Entity("AirWaze.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Bus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AirWaze.Entities.Destination", b =>
                {
                    b.Property<int>("DestinationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DestinationID"), 1L, 1);

                    b.Property<decimal>("Distance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan>("FlightTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DestinationID");

                    b.ToTable("Destinations");
                });

            modelBuilder.Entity("AirWaze.Entities.Flight", b =>
                {
                    b.Property<int>("FlightID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightID"), 1L, 1);

                    b.Property<int>("CurrentGateGateID")
                        .HasColumnType("int");

                    b.Property<bool>("CurrentPlaneConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("CurrentPlanePlaneID")
                        .HasColumnType("int");

                    b.Property<int>("CurrentRunwayRunwayID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Departure")
                        .HasColumnType("datetime2");

                    b.Property<int>("DestinationID")
                        .HasColumnType("int");

                    b.Property<string>("FlightNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeatDiagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("FlightID");

                    b.HasIndex("CurrentGateGateID");

                    b.HasIndex("CurrentPlanePlaneID");

                    b.HasIndex("CurrentRunwayRunwayID");

                    b.HasIndex("DestinationID");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("AirWaze.Entities.Gate", b =>
                {
                    b.Property<int>("GateID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GateID"), 1L, 1);

                    b.Property<double>("CoordsLat")
                        .HasColumnType("float");

                    b.Property<double>("CoordsLon")
                        .HasColumnType("float");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("GateID");

                    b.ToTable("Gates");
                });

            modelBuilder.Entity("AirWaze.Entities.Plane", b =>
                {
                    b.Property<int>("PlaneID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlaneID"), 1L, 1);

                    b.Property<int>("AirMiles")
                        .HasColumnType("int");

                    b.Property<DateTime>("ConstructionYear")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CurrentAirlineAirlineID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FirstClassCapacity")
                        .HasColumnType("int");

                    b.Property<int>("FlightHours")
                        .HasColumnType("int");

                    b.Property<string>("FlightRegion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FuelCapacity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FuelUsagePerKM")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<decimal>("LoadCapacity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NextMainentance")
                        .HasColumnType("int");

                    b.Property<int>("PassengerCapacity")
                        .HasColumnType("int");

                    b.Property<string>("PlaneNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeatDiagramPic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlaneID");

                    b.HasIndex("CurrentAirlineAirlineID");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("AirWaze.Entities.Runway", b =>
                {
                    b.Property<int>("RunwayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RunwayID"), 1L, 1);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("RunwayID");

                    b.ToTable("Runways");
                });

            modelBuilder.Entity("AirWaze.Entities.Ticket", b =>
                {
                    b.Property<int>("TicketID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketID"), 1L, 1);

                    b.Property<int>("CurrentFlightFlightID")
                        .HasColumnType("int");

                    b.Property<string>("CurrentUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ExtraLuggage")
                        .HasColumnType("bit");

                    b.Property<bool>("FirstClass")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Seat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TicketNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TicketID");

                    b.HasIndex("CurrentFlightFlightID");

                    b.HasIndex("CurrentUserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("AirWaze.Entities.Flight", b =>
                {
                    b.HasOne("AirWaze.Entities.Gate", "CurrentGate")
                        .WithMany()
                        .HasForeignKey("CurrentGateGateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirWaze.Entities.Plane", "CurrentPlane")
                        .WithMany()
                        .HasForeignKey("CurrentPlanePlaneID");

                    b.HasOne("AirWaze.Entities.Runway", "CurrentRunway")
                        .WithMany()
                        .HasForeignKey("CurrentRunwayRunwayID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirWaze.Entities.Destination", "Destination")
                        .WithMany()
                        .HasForeignKey("DestinationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentGate");

                    b.Navigation("CurrentPlane");

                    b.Navigation("CurrentRunway");

                    b.Navigation("Destination");
                });

            modelBuilder.Entity("AirWaze.Entities.Plane", b =>
                {
                    b.HasOne("AirWaze.Entities.Airline", "CurrentAirline")
                        .WithMany("CurrentPlanes")
                        .HasForeignKey("CurrentAirlineAirlineID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentAirline");
                });

            modelBuilder.Entity("AirWaze.Entities.Ticket", b =>
                {
                    b.HasOne("AirWaze.Entities.Flight", "CurrentFlight")
                        .WithMany()
                        .HasForeignKey("CurrentFlightFlightID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirWaze.Entities.ApplicationUser", "CurrentUser")
                        .WithMany()
                        .HasForeignKey("CurrentUserId");

                    b.Navigation("CurrentFlight");

                    b.Navigation("CurrentUser");
                });

            modelBuilder.Entity("AirWaze.Entities.Airline", b =>
                {
                    b.Navigation("CurrentPlanes");
                });
#pragma warning restore 612, 618
        }
    }
}
