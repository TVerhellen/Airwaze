using AirWaze.Areas.Identity.Data;
using AirWaze.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirWaze.Data;

public class AirWazeContext : IdentityDbContext<AirWazeUser>
{
    public AirWazeContext(DbContextOptions<AirWazeContext> options)
        : base(options)
    {
    }

    public DbSet<AirWazeUser> AirWazeUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
