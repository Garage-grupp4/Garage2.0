using Garage2._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Data;

public static class SeedData
{
    public static void Initialize(_1 db)
    {
        Console.WriteLine("→ Kontrollerar databasen...");
        db.Database.Migrate();

        if (db.ParkedVehicle.Any())
        {
            var count = db.ParkedVehicle.Count();
            Console.WriteLine($"✓ Databasen innehåller redan {count} fordon — hoppar över seed.");
            return;
        }

        Console.WriteLine("→ Tom databas hittad. Seedar testdata...");
        db.ParkedVehicle.AddRange(
            new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegistrationNumber = "ABC123",
                Color = "Röd",
                VehicleBrand = "Volvo",
                VehicleModel = "V70",
                Wheels = 4,
                ArrivalTime = DateTime.Now.AddHours(-3),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Motorcycle,
                RegistrationNumber = "MC0001",
                Color = "Svart",
                VehicleBrand = "Yamaha",
                VehicleModel = "MT-07",
                Wheels = 2,
                ArrivalTime = DateTime.Now.AddHours(-1),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegistrationNumber = "DEF456",
                Color = "Blå",
                VehicleBrand = "Saab",
                VehicleModel = "9-3",
                Wheels = 4,
                ArrivalTime = DateTime.Now.AddHours(-8),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegistrationNumber = "GHI789",
                Color = "Vit",
                VehicleBrand = "Tesla",
                VehicleModel = "Model 3",
                Wheels = 4,
                ArrivalTime = DateTime.Now.AddMinutes(-45),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegistrationNumber = "JKL012",
                Color = "Silver",
                VehicleBrand = "Volkswagen",
                VehicleModel = "Golf",
                Wheels = 4,
                ArrivalTime = DateTime.Now.AddDays(-1),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Motorcycle,
                RegistrationNumber = "MNO345",
                Color = "Grön",
                VehicleBrand = "Kawasaki",
                VehicleModel = "Ninja 650",
                Wheels = 2,
                ArrivalTime = DateTime.Now.AddHours(-5),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Bus,
                RegistrationNumber = "BUS001",
                Color = "Gul",
                VehicleBrand = "Scania",
                VehicleModel = "Citywide",
                Wheels = 6,
                ArrivalTime = DateTime.Now.AddHours(-2),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Truck,
                RegistrationNumber = "LKW999",
                Color = "Röd",
                VehicleBrand = "Volvo",
                VehicleModel = "FH16",
                Wheels = 8,
                ArrivalTime = DateTime.Now.AddHours(-12),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Car,
                RegistrationNumber = "PQR678",
                Color = "Svart",
                VehicleBrand = "BMW",
                VehicleModel = "M3",
                Wheels = 4,
                ArrivalTime = DateTime.Now.AddMinutes(-15),
            },
            new ParkedVehicle
            {
                VehicleType = VehicleType.Truck,
                RegistrationNumber = "TRK500",
                Color = "Vit",
                VehicleBrand = "Scania",
                VehicleModel = "R500",
                Wheels = 6,
                ArrivalTime = DateTime.Now.AddHours(-6),
            }
        );

        var added = db.SaveChanges();
        Console.WriteLine($"✓ Seed klar — {added} fordon tillagda.");
    }
}
