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

    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
        builder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable(name: "AirWazeUser");
        });
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "AirWazeRole");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("AirWazeUserRoles");
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("AirWazeUserClaims");
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("AirWazeUserLogins");
        });
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("AirWazeRoleClaims");
        });
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("AirWazeUserTokens");
        });
    }
}
