using Garage2._0.Models;
using Microsoft.EntityFrameworkCore;

public class GarageContext(DbContextOptions<GarageContext> options) : DbContext(options)
{
    public DbSet<Garage2._0.Models.Vehicle> ParkedVehicle { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>().HasIndex(p => p.RegistrationNumber).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}
