using Garage2._0.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Data;

public class GarageContext(DbContextOptions<GarageContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ParkedVehicle>().HasIndex(p => p.RegistrationNumber).IsUnique();

        modelBuilder.Entity<ApplicationUser>()
          .HasIndex(u => u.PersonNumber)
          .IsUnique();

    }
}
