using Garage2._0.Models;
using Microsoft.EntityFrameworkCore;

public class _1(DbContextOptions<_1> options) : DbContext(options)
{
    public DbSet<Garage2._0.Models.ParkedVehicle> ParkedVehicle { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkedVehicle>().HasIndex(p => p.RegistrationNumber).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}
