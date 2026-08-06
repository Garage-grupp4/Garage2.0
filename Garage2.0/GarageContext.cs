using Garage2._0.Models;
using Microsoft.EntityFrameworkCore;

public class GarageContext(DbContextOptions<GarageContext> options) : DbContext(options)
{
    public DbSet<Garage2._0.Models.Vehicle> ParkedVehicle { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>().HasIndex(p => p.RegistrationNumber).IsUnique();

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

        base.OnModelCreating(modelBuilder);
    }
}
