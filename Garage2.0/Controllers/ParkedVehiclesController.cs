using Microsoft.EntityFrameworkCore;
using Garage2._0.Data;
using Garage2._0.Models;
using Garage2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Garage2._0.Controllers;

[Authorize]
public class ParkedVehiclesController : Controller  //viewmodel för att visa en lista med parkerade fordon, med möjlighet att filtrera efter registreringsnummer och fordonstyp.
{
    private readonly GarageContext _context;

    public ParkedVehiclesController(GarageContext context)
    {
        _context = context;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index(string sort, string license, string type)
    {
        IQueryable<Vehicle> vehicles = _context.Vehicles; // Query the database for all parked vehicles. Removed select and var to avoid unnecessary data retrieval from the database.

        var user = GetApplicationUser();
        if (!IsAdmin())
            vehicles = vehicles.Where(v => (v.OwnerId ?? "") == user.Id);

        ViewData["vehicletypes"] = _context.VehicleTypes.Select(t => t.Name).ToArray();
        // Save search terms to populate html page
        ViewData["license"] = license;
        ViewData["type"] = type;
        ViewData["sort"] = sort;

        // Filter with search terms
        if (!string.IsNullOrEmpty(license))
            vehicles = vehicles.Where(v => v.RegistrationNumber.ToUpper().StartsWith(license.ToUpper())); // changed to startwith to make it more user friendly kanske använda Equals instead of ToUpper() for exact match, but then it would be case sensitive. Could use ToLower() instead of ToUpper() for case insensitive match.
        if (!string.IsNullOrEmpty(type))
            vehicles = vehicles.Where(v => v.VehicleType.Name == type);

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

        var viewModel = await vehicles.Select(v => new VehicleOverViewModel // 
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            VehicleType = v.VehicleType.Name,
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
    public async Task<IActionResult> Details(int? id) //kan göra lite snyggare här
    {
        if (id == null)
            return NotFound();

        var parkedvehicle = await _context.Vehicles
            .Include(v => v.Owner)
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (parkedvehicle == null)
            return NotFound();
        if (!IsAuthorized(parkedvehicle))
            return Unauthorized();

        return View(parkedvehicle);
    }

    // GET: PARKEDVEHICLES/Create
    public IActionResult Create()
    {
        ViewData["vehicletypes"] = _context.VehicleTypes.ToArray();
        return View();
    }

    // POST: PARKEDVEHICLES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateParkedVehicleViewModel model)
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
        ApplicationUser user = GetApplicationUser();
        Vehicle newParkedVehicle = new Vehicle()
        {
            //ArrivalTime = DateTime.Now, ToDo (In later task): set ArrivalTime by creating a ParkingSession here once parking flow exists
            RegistrationNumber = NormalizeRegistrationNumber(model.RegistrationNumber),
            VehicleBrand = model.VehicleBrand,
            VehicleModel = model.VehicleModel,
            VehicleType = _context.VehicleTypes.First(t => t.Id == model.VehicleTypeId),
            Color = model.Color,
            Wheels = model.Wheels,
            OwnerId = user.Id,
            Owner = user
        };

        // Send to Database
        if (ModelState.IsValid)
        {
            _context.Add(newParkedVehicle);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Successfully Parked {newParkedVehicle}"; // at {newParkedVehicle.ArrivalTime}";
            return RedirectToAction(nameof(Index));
        }
        ViewData["Error"] = "Error message text.";
        return View();
    }

    private async Task<bool> IsRegistrationNumberUnique(string registrationNumber)
    {
        return !await _context.Vehicles
            .AnyAsync(v => v.RegistrationNumber == registrationNumber);
    }

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
        if (id == null)
        {
            return NotFound();
        }

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(m => m.Id == id);

        if (vehicle == null)
        {
            return NotFound();
        }

        return View(vehicle);
    }

    // POST: PARKEDVEHICLES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);

        if (vehicle == null) return NotFound();
        if (!IsAuthorized(vehicle)) return Unauthorized();

        var receipt = new ReceiptViewModel
        {
            Id = vehicle.Id,
            RegistrationNumber = vehicle.RegistrationNumber,
            VehicleType = vehicle.VehicleType,
            VehicleBrand = vehicle.VehicleBrand,
            VehicleModel = vehicle.VehicleModel,
            Color = vehicle.Color,
            Wheels = vehicle.Wheels,
            //ArrivalTime = vehicle.ArrivalTime,  TODO (In Later Task): ArrivalTime/DepartureTime now come from the active ParkingSession, not Vehicle — checkout flow needs rework
            DepartureTime = DateTime.Now
        };

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Successfully check out {vehicle} ";
        return View("Receipt", receipt);
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
        var user = GetApplicationUser();
        return user.PersonNumber != "111111111-1111"; // TODO: change to role based check
    }
    private bool IsAuthorized(Vehicle vehicle)
    {
        return IsAdmin() || vehicle.OwnerId == GetApplicationUser().Id;
    }
}
