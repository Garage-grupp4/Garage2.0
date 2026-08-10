using Garage2._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Data;

public static class SeedData
{
    public static void Initialize(GarageContext db)
    {
        Console.WriteLine("→ Kontrollerar databasen...");
        db.Database.Migrate();

        if (db.Vehicles.Any())
        {
            var count = db.Vehicles.Count();
            Console.WriteLine($"✓ Databasen innehåller redan {count} fordon — hoppar över seed.");
            return;
        }

        Console.WriteLine("→ Tom databas hittad. Seedar testdata...");

        var carType = new VehicleType { Name = "Car" };
        var motorcycleType = new VehicleType { Name = "Motorcycle" };
        var busType = new VehicleType { Name = "Bus" };
        var truckType = new VehicleType { Name = "Truck" };

        db.VehicleTypes.AddRange(carType, motorcycleType, busType, truckType);
        db.SaveChanges();

        var spots = new List<ParkingSpot>();
        for (int i = 2; i <= 40; i += 2)
        {
            spots.Add(new ParkingSpot { Number = i, Location = "Garage A", isOutOfService = false });
        }

        db.ParkingSpots.AddRange(spots);    
        db.SaveChanges();

        var added = db.SaveChanges();
        Console.WriteLine($"✓ Seed klar — {added} fordon tillagda.");
    }
}
