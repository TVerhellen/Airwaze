using AirWaze.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirWaze.Database
{
    public class AirWazeDbContext: DbContext
    {
        public AirWazeDbContext(DbContextOptions<AirWazeDbContext> options) : base (options)
        {

        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Plane> Planes { get; set; }

        public DbSet<Gate> Gates { get; set; }

        public DbSet<Runway> Runways { get; set; }

    }
}
