using AirWaze.Areas.Identity.Data;
using AirWaze.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirWaze.Data
{
    public class ApplicationDbContext : IdentityDbContext<AirWazeUser>
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AirWazeUser> AirWazeUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.HasDefaultSchema("Identity");
            //builder.Entity<IdentityUser>(entity =>
            //{
            //    entity.ToTable(name: "AirWazeUser");
            //});
            //builder.Entity<IdentityRole>(entity =>
            //{
            //    entity.ToTable(name: "AirWazeRole");
            //});
            //builder.Entity<IdentityUserRole<string>>(entity =>
            //{
            //    entity.ToTable("AirWazeUserRoles");
            //});
            //builder.Entity<IdentityUserClaim<string>>(entity =>
            //{
            //    entity.ToTable("AirWazeUserClaims");
            //});
            //builder.Entity<IdentityUserLogin<string>>(entity =>
            //{
            //    entity.ToTable("AirWazeUserLogins");
            //});
            //builder.Entity<IdentityRoleClaim<string>>(entity =>
            //{
            //    entity.ToTable("AirWazeRoleClaims");
            //});
            //builder.Entity<IdentityUserToken<string>>(entity =>
            //{
            //    entity.ToTable("AirWazeUserTokens");
            //});
        }
    }
}