using Garage2._0.Constants;
using Microsoft.EntityFrameworkCore;
using Garage2._0.Data;
using Garage2._0.Models;
using Garage2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Garage2._0.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace Garage2._0.Controllers;

[Authorize]
public class ParkedVehiclesController : Controller  //viewmodel för att visa en lista med parkerade fordon, med möjlighet att filtrera efter registreringsnummer och fordonstyp.
{
    private readonly GarageContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVehicleDropDownService vehicleDropDownService;
    private readonly IOptions<ParkingPricingOptions> _pricingOptions;

    public ParkedVehiclesController(
        GarageContext context, 
        UserManager<ApplicationUser> userManager, 
        IVehicleDropDownService vehicleDropDownService, 
        IOptions<ParkingPricingOptions> pricingOptions)
    {
        _context = context;
        _userManager = userManager;
        this.vehicleDropDownService = vehicleDropDownService;
        _pricingOptions = pricingOptions;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index(string sort, string license, int? type)//ToDo int type from search 
    {
        IQueryable<Vehicle> vehicles = _context.Vehicles; // Query the database for all parked vehicles. Removed select and var to avoid unnecessary data retrieval from the database.

        var user = GetApplicationUser();
        if (!IsAdmin())
            vehicles = vehicles.Where(v => (v.ApplicationUserId ?? "") == user.Id);

        ViewData["vehicletypes"] = _context.VehicleTypes.Select(t => t.Name).ToArray();
        // Save search terms to populate html page
        ViewData["license"] = license;
        ViewData["type"] = type;
        ViewData["sort"] = sort;

        // Filter with search terms
        if (!string.IsNullOrEmpty(license))
            vehicles = vehicles.Where(v => v.RegistrationNumber.ToUpper().StartsWith(license.ToUpper())); // changed to startwith to make it more user friendly kanske använda Equals instead of ToUpper() for exact match, but then it would be case sensitive. Could use ToLower() instead of ToUpper() for case insensitive match.
        if (type != null)
            vehicles = vehicles.Where(v => v.VehicleTypeId == type);

        // Sort by Vehicle Type, Registration number, Arrival time, Time Parked
        switch (sort)
        {
            case "license":
                vehicles = vehicles.OrderBy(v => v.RegistrationNumber);
                break;
            case "license_d":
                vehicles = vehicles.OrderByDescending(v => v.RegistrationNumber);
                break;
            case "type":
                vehicles = vehicles.OrderBy(v => v.VehicleType);
                break;
            case "type_d":
                vehicles = vehicles.OrderByDescending(v => v.VehicleType);
                break;
            case "start":
                vehicles = vehicles.OrderBy(v => 
                v.ParkingSessions
                    .Where(ps => ps.DepartureTime == null)
                    .Select(ps => ps.ArrivalTime)
                    .FirstOrDefault());
                break;
            case "start_d":
                vehicles = vehicles.OrderByDescending(v => 
                v.ParkingSessions
                    .Where(ps => ps.DepartureTime == null)
                    .Select(ps => ps.ArrivalTime)
                    .FirstOrDefault());
                break;
        }

        var viewModel = await vehicles.Select(v => new VehicleOverViewModel  
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            VehicleType = v.VehicleType,
            ArrivalTime = v.ParkingSessions
                .Where(ps => ps.DepartureTime == null)
                .Select(ps => (DateTime?)ps.ArrivalTime)
                .FirstOrDefault(),

        }).ToListAsync();

        if (sort == "parked")
            viewModel = viewModel.OrderBy(v => v.ParkedDuration).ToList();
        else if (sort == "parked_d")
            viewModel = viewModel.OrderByDescending(v => v.ParkedDuration).ToList();

        return View(nameof(Index), viewModel);
    }

    //GET: PARKEDVEHICLES/Details/5
    public async Task<IActionResult> Details(int? id) 
    {
        if (id == null)
            return NotFound();

        var parkedvehicle = await _context.Vehicles
            .Include(v => v.ApplicationUser)
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (parkedvehicle == null)
            return NotFound();
        if (!IsAuthorized(parkedvehicle))
            return Unauthorized();

        return View(parkedvehicle);
    }

    // GET: PARKEDVEHICLES/Register
    public async Task<IActionResult> Register()
    {
        return View();
    }
    public async Task<IActionResult> Park()
    {
        var model = new ParkViewModel
        {
            Vehicles = await vehicleDropDownService.GetVehicleSelectListAsync(_userManager.GetUserId(User))

        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Park(ParkViewModel model)
    {
        var user =await _userManager.GetUserAsync(User);  
        var birthDate = user?.PersonNumberToDateTime();  
        if (birthDate == null)  
        {  
            string error = $"Could not get personNumber data.";  
            ModelState.AddModelError(nameof(user.PersonNumber), error);  
            TempData["Error"] = error;  
            return View(model);  
        }  
        var aliveTime = DateTime.Now.Subtract(birthDate.Value);  
        if (aliveTime.TotalDays/365 < 18)  
        {  
            string warning = $"User is under 18 years old.";  
            ModelState.AddModelError(nameof(user.PersonNumber), warning);  
            TempData["Warning"] = warning;  
            return View(model);  
        }
        if (ModelState.IsValid)
        {
            var vehicle = await _context.Vehicles.FindAsync(model.SelectedVehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            var spot = await GetFirstAvailableParkingSpot();
            if (spot == null)
            {
                ModelState.AddModelError(string.Empty, "No available parking spots.");
                return View(model);
            }

            var session = new ParkingSession
            {
                VehicleId = vehicle.Id,
                ParkingSpotId = spot.Id,
                ArrivalTime = DateTime.Now,
                HourlyRateForParking = _pricingOptions.Value.HourlyRate
            };

            _context.ParkingSessions.Add(session);
            await _context.SaveChangesAsync();


            TempData["Success"] = $"Successfully parked {vehicle.RegistrationNumber} in spot {spot.Number}.";
            return RedirectToAction(nameof(Index));

        }
        return View(model);
    }

 

    // POST: PARKEDVEHICLES/Register
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CreateParkedVehicleViewModel model)
    {
        // Validate if unique number or return model error
        if (!await IsRegistrationNumberUnique(model.RegistrationNumber))
        {
            string warning = $"Registration number {model.RegistrationNumber} is already in use.";
            ModelState.AddModelError(nameof(model.RegistrationNumber), warning);
            TempData["Warning"] = warning;
            return View(model);
        }

        // Generate newVehicle
        Vehicle newRegistredVehicle = new Vehicle()
        {
            //ArrivalTime = DateTime.Now, ToDo (In later task): set ArrivalTime by creating a ParkingSession here once parking flow exists
            ApplicationUserId = _userManager.GetUserId(User),
            RegistrationNumber = model.RegistrationNumber,
            VehicleBrand = model.VehicleBrand,
            VehicleModel = model.VehicleModel,
            VehicleTypeId = model.VehicleTypeId,
            Color = model.Color,
            Wheels = model.Wheels
        };

        // Send to Database
        if (ModelState.IsValid)
        {
            _context.Add(newRegistredVehicle);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Successfully Registred {newRegistredVehicle}"; 
            return RedirectToAction(nameof(Index));
        }
        ViewData["Error"] = "Error message text.";
        return View(model);
    }

    private async Task<bool> IsRegistrationNumberUnique(string registrationNumber)
    {
        return !await _context.Vehicles
            .AnyAsync(v => v.RegistrationNumber == registrationNumber);
    }

    private async Task<ParkingSpot?> GetFirstAvailableParkingSpot()
    {
             return await _context.ParkingSpots
            .Where(ps => !ps.ParkingSessions.Any(pss => pss.DepartureTime == null)) // Only include spots that are not currently occupied
            .Where(ps => !ps.IsOutOfService)
            .OrderBy(ps => ps.Number) // Optional: order by Id to get the first available spot
            .FirstOrDefaultAsync();
    }

    //private async Task<List<Vehicle>> GetAvailableVehiclesForUser(string userId) 
    //{
    //    return await _context.Vehicles
    //        .Where(v => v.ApplicationUserId == userId)
    //        .Where(v => !v.ParkingSessions.Any(ps => ps.DepartureTime == null)) // Only include vehicles that are not currently parked
    //        .ToListAsync();
    //}

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyRegistrationNumber(string registationNumber)
    {
        IEnumerable<Vehicle> list = await _context.Vehicles.ToListAsync();
        if (list.Any(v => v.RegistrationNumber == registationNumber))
        {
            return Json($"Email {registationNumber} is already in use.");
        }
        return Json(true);
    }

    // GET: PARKEDVEHICLES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var parkedvehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (parkedvehicle == null)
        {
            return NotFound();
        }
        if (!IsAuthorized(parkedvehicle))
            return Unauthorized();

        var parkedvehicleModel = new EditParkedVehicleViewModel()
        {
            Id = parkedvehicle.Id,
            RegistrationNumber = parkedvehicle.RegistrationNumber,
            OriginalRegistrationNumber = parkedvehicle.RegistrationNumber,
            Color = parkedvehicle.Color ?? "#ffffff",
            VehicleBrand = parkedvehicle.VehicleBrand,
            //ArrivalTime = parkedvehicle.ArrivalTime, TODO: ArrivalTime removed — no longer belongs on Vehicle, it's on ParkingSession now (can not be edited here)
            VehicleModel = parkedvehicle.VehicleModel,
            VehicleTypeId = parkedvehicle.VehicleType.Id,
            Wheels = parkedvehicle.Wheels,
        };
        return View(parkedvehicleModel);
    }

    // POST: PARKEDVEHICLES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, EditParkedVehicleViewModel model)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Validate if unique number or return model error
        if (model.OriginalRegistrationNumber != model.RegistrationNumber && !await IsRegistrationNumberUnique(model.RegistrationNumber))
        {
            string warning = $"Registration number {model.RegistrationNumber} is already in use.";
            ModelState.AddModelError(nameof(model.RegistrationNumber), warning);
            TempData["Warning"] = warning;
            return View(model);
        }

        Vehicle? parkedVehicle = _context.Vehicles.FirstOrDefault(p => p.Id ==id);
        if (parkedVehicle == null) return NotFound();
        if (!IsAuthorized(parkedVehicle)) return Unauthorized();

        parkedVehicle.RegistrationNumber = NormalizeRegistrationNumber(model.RegistrationNumber);
        parkedVehicle.VehicleBrand = model.VehicleBrand;
        parkedVehicle.VehicleModel = model.VehicleModel;
        parkedVehicle.VehicleType = _context.VehicleTypes.FirstOrDefault(t => t.Id == model.VehicleTypeId);
        parkedVehicle.Color = model.Color;
        parkedVehicle.Wheels = model.Wheels;

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(parkedVehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(parkedVehicle.Id))
                {
                    TempData["Error"] = "Vehicle not found";
                    return NotFound();
                }
                else
                {
                    TempData["Error"] = "DbUpdateConcurrencyException";
                    throw;
                }
            }
            TempData["Success"] = $"Successfully edited Vehicle {parkedVehicle}";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    // GET: PARKEDVEHICLES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var parkingSession = await _context.ParkingSessions
            .Include(p => p.Vehicle)
            .ThenInclude(v => v.VehicleType)
            .Include(p => p.ParkingSpot)
            .FirstOrDefaultAsync(p => p.VehicleId == id && p.DepartureTime == null);

        if (parkingSession == null) return NotFound();

        if(!IsAuthorized(parkingSession.Vehicle)) return Unauthorized();
        
        return View(parkingSession.Vehicle);
    }

    // POST: PARKEDVEHICLES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null) return NotFound();

        var parkingSession = await _context.ParkingSessions
            .Include(p => p.Vehicle)
            .ThenInclude(v => v.ApplicationUser)
            .Include(p => p.ParkingSpot)
            .Include(parkingSession => parkingSession.Vehicle)
            .ThenInclude(vehicle => vehicle.VehicleType)
            .FirstOrDefaultAsync(p => p.VehicleId == id && p.DepartureTime == null);

        if(parkingSession == null) return NotFound();

        if(!IsAuthorized(parkingSession.Vehicle)) return Unauthorized();
        
        parkingSession.DepartureTime = DateTime.Now;

        var duration = parkingSession.DepartureTime.Value - parkingSession.ArrivalTime;
        var totalHours = (decimal)Math.Ceiling(duration.TotalHours);
        if(totalHours < 1) totalHours = 1; // Minimum charge for 1 hour)

        parkingSession.TotalCost = totalHours * parkingSession.HourlyRateForParking;

        await _context.SaveChangesAsync();

        var receiptViewModel = new ReceiptViewModel
        {
        Id = parkingSession.Vehicle.Id,
        RegistrationNumber = parkingSession.Vehicle.RegistrationNumber,
        VehicleType = parkingSession.Vehicle.VehicleType,
        VehicleBrand = parkingSession.Vehicle.VehicleBrand,
        VehicleModel = parkingSession.Vehicle.VehicleModel,
        Color = parkingSession.Vehicle.Color,
        Wheels = parkingSession.Vehicle.Wheels,
        ArrivalTime = parkingSession.ArrivalTime,
        DepartureTime = parkingSession.DepartureTime.Value,
        TotalPrice = parkingSession.TotalCost.Value,

        UserFirstName = parkingSession.Vehicle.ApplicationUser.FirstName,
        UserLastName = parkingSession.Vehicle.ApplicationUser.LastName,
        ParkingSpotNumber = parkingSession.ParkingSpot.Number,
        HourlyRate = parkingSession.HourlyRateForParking,   

        };

        TempData["Success"] = $"Successfully checked out {parkingSession.Vehicle.RegistrationNumber} from spot {parkingSession.ParkingSpot.Number}.";
        return View("Receipt", receiptViewModel);
    }

    private bool ParkedVehicleExists(int? id)
    {
        return _context.Vehicles.Any(e => e.Id == id);
    }

    private ApplicationUser GetApplicationUser()
    {
        return _context.Users.First(u => u.UserName == (User.Identity != null ? User.Identity.Name : ""));
    }
    private static string NormalizeRegistrationNumber(string registrationNumber)
    {
        return registrationNumber.Trim().ToUpper();
    }
    private bool IsAdmin()
    {
        var roles = _userManager.GetRolesAsync(GetApplicationUser()).Result;
        return roles.Contains(Roles.ADMIN); 
    }
    private bool IsAuthorized(Vehicle vehicle)
    {
        return IsAdmin() || vehicle.ApplicationUserId == GetApplicationUser().Id;
    }
}
