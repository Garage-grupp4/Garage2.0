using Garage2._0.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Data;

public class GarageContext(DbContextOptions<GarageContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Garage2._0.Models.Vehicle> Vehicles { get; set; } = default!;
    public DbSet<Garage2._0.Models.VehicleType> VehicleTypes { get; set; } = default!;
    public DbSet<Garage2._0.Models.ParkingSpot> ParkingSpots { get; set; } = default!;

    public DbSet<Garage2._0.Models.ParkingSession> ParkingSessions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>().HasIndex(p => p.RegistrationNumber).IsUnique();

        modelBuilder.Entity<ParkingSpot>().HasIndex(p => p.Number).IsUnique();

        modelBuilder.Entity<VehicleType>().HasIndex(vt => vt.Name).IsUnique();


        modelBuilder.Entity<ParkingSession>()
            .HasOne(ps => ps.Vehicle)
            .WithMany(v => v.ParkingSessions)
            .HasForeignKey(ps => ps.VehicleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ParkingSession>()
            .HasOne(ps => ps.ParkingSpot)
            .WithMany(sp => sp.ParkingSessions)
            .HasForeignKey(ps => ps.ParkingSpotId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.VehicleType)
            .WithMany(vt => vt.Vehicles)
            .HasForeignKey(v => v.VehicleTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vehicle>()
                    .HasOne(v => v.Owner)
                    .WithMany(u => u.Vehicles)
                    .HasForeignKey(v => v.OwnerId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
        
        base.OnModelCreating(modelBuilder);
    }
}
