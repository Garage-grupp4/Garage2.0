using Garage2._0.Constants;
using Garage2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Data;

public static class SeedData
{
    public static async Task Initialize(GarageContext db, IServiceProvider services)
    {
        Console.WriteLine("→ Kontrollerar databasen...");
        db.Database.Migrate();

        if (!db.Roles.Any())
        {
            var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedRoles(db, _roleManager);
            await SeedAdminUser(db, _userManager);
        }

        if (db.Vehicles.Any())
        {
            var count = db.Vehicles.Count();
            Console.WriteLine($"✓ Databasen innehåller redan {count} fordon — hoppar över seed.");
            return;
        }

        Console.WriteLine("→ Tom databas hittad. Seedar testdata...");
        if (!db.VehicleTypes.Any())
        {
            var carType = new VehicleType { Name = "Car" };
            var motorcycleType = new VehicleType { Name = "Motorcycle" };
            var busType = new VehicleType { Name = "Bus" };
            var truckType = new VehicleType { Name = "Truck" };

            db.VehicleTypes.AddRange(carType, motorcycleType, busType, truckType);
            db.SaveChanges();
        }

        if (!db.ParkingSpots.Any())
        {
            var spots = new List<ParkingSpot>();
            for (int i = 2; i <= 40; i += 2)
            {
                spots.Add(new ParkingSpot { Number = i, Location = "Garage A", IsOutOfService = false });
            }

            db.ParkingSpots.AddRange(spots);
            db.SaveChanges();
        }

        var added = db.SaveChanges();
        Console.WriteLine($"✓ Seed klar — {added} fordon tillagda.");
    }

    private static async Task SeedRoles(GarageContext db, RoleManager<IdentityRole> roleManager)
    {

        var roles = new[] { Roles.ADMIN, Roles.MEMBER };
        foreach (string roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;

            var role = new IdentityRole { Name = roleName };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task SeedAdminUser(GarageContext db, UserManager<ApplicationUser> userManager)
    {
        var admin = await AddAccountAsync(userManager,
            "admin@admin.com",
            "adminuser",
            "Adminsson",
            "password1!",
            "111111-1111");

        await userManager.AddToRoleAsync(admin, Roles.ADMIN);
    }


    private static async Task<ApplicationUser> AddAccountAsync(UserManager<ApplicationUser> userManager, 
        string accountEmail, 
        string fName, 
        string lName, 
        string pw,
        string personNumber)
    {
        var found = await userManager.FindByEmailAsync(accountEmail);

        // Returns if exists, maybe give warning in the future instead
        if (found != null) return found;

        ApplicationUser user = new ApplicationUser
        {
            UserName = accountEmail,
            Email = accountEmail,
            FirstName = fName,
            LastName = lName,
            PersonNumber = personNumber,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, pw);

        if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

        return user;
    }
}
