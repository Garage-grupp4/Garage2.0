using Garage2._0.Models;
using Garage2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Controllers;

public class ParkedVehiclesController : Controller  //viewmodel för att visa en lista med parkerade fordon, med möjlighet att filtrera efter registreringsnummer och fordonstyp.
{
    private readonly _1 _context;

    public ParkedVehiclesController(_1 context)
    {
        _context = context;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index(string sort, string license, VehicleType? type)
    {
        IQueryable<ParkedVehicle> vehicles = _context.ParkedVehicle; // Query the database for all parked vehicles. Removed select and var to avoid unnecessary data retrieval from the database.

        // Save search terms to populate html page
        ViewData["license"] = license;
        ViewData["type"] = type;
        ViewData["sort"] = sort;

        // Filter with search terms
        if (!string.IsNullOrEmpty(license))
            vehicles = vehicles.Where(v => v.RegistrationNumber.ToUpper().StartsWith(license.ToUpper())); // changed to startwith to make it more user friendly kanske använda Equals instead of ToUpper() for exact match, but then it would be case sensitive. Could use ToLower() instead of ToUpper() for case insensitive match.
        if (type != null)
            vehicles = vehicles.Where(v => v.VehicleType == type);

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
                vehicles = vehicles.OrderBy(v => v.ArrivalTime);
                break;
            case "start_d":
                vehicles = vehicles.OrderByDescending(v => v.ArrivalTime);
                break;
        }
        
        var viewModel = await vehicles.Select(v => new VehicleOverViewModel // 
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            VehicleType = v.VehicleType,
            ArrivalTime = v.ArrivalTime 
        
        }).ToListAsync();

        if (sort == "parked")
            viewModel = viewModel.OrderBy(v => v.ParkedDuration).ToList();
        else if (sort == "parked_d")
            viewModel = viewModel.OrderByDescending(v => v.ParkedDuration).ToList();

        return View(viewModel);
    }

    //GET: PARKEDVEHICLES/Details/5
    public async Task<IActionResult> Details(int? id) //kan göra lite snyggare här
    {
        if (id == null)
        {
            return NotFound();
        }

        var parkedvehicle = await _context.ParkedVehicle
            .FirstOrDefaultAsync(m => m.Id == id);
        if (parkedvehicle == null)
        {
            return NotFound();
        }

        return View(parkedvehicle);
    }

    // GET: PARKEDVEHICLES/Create
    public IActionResult Create()
    {
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
        ParkedVehicle newParkedVehicle = new ParkedVehicle()
        {
            ArrivalTime = DateTime.Now,
            RegistrationNumber = model.RegistrationNumber,
            VehicleBrand = model.VehicleBrand,
            VehicleModel = model.VehicleModel,
            VehicleType = model.VehicleType,
            Color = model.Color,
            Wheels = model.Wheels
        };
        
        // Send to Database
        if (ModelState.IsValid)
        {
            _context.Add(newParkedVehicle);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Successfully Parked {newParkedVehicle} at {newParkedVehicle.ArrivalTime}";
            return RedirectToAction(nameof(Index));
        }
        ViewData["Error"] = "Error message text."; 
        return View();
    }
    
    private async Task<bool> IsRegistrationNumberUnique(string registrationNumber)
    {
        return !await _context.ParkedVehicle
            .AnyAsync(v => v.RegistrationNumber == registrationNumber);
    }
    
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyRegistrationNumber(string registationNumber)
    {
        IEnumerable<ParkedVehicle> list = await _context.ParkedVehicle.ToListAsync();
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

        var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);
        if (parkedvehicle == null)
        {
            return NotFound();
        }

        var parkedvehicleModel = new EditParkedVehicleViewModel()
        {
            Id = parkedvehicle.Id,
            RegistrationNumber = parkedvehicle.RegistrationNumber,
            OriginalRegistrationNumber = parkedvehicle.RegistrationNumber,
            Color = parkedvehicle.Color ?? "#ffffff",
            VehicleBrand = parkedvehicle.VehicleBrand,
            ArrivalTime = parkedvehicle.ArrivalTime,
            VehicleModel = parkedvehicle.VehicleModel,
            VehicleType = parkedvehicle.VehicleType,
            Wheels = parkedvehicle.Wheels,
        };
        return View(parkedvehicleModel);
    }

    // POST: PARKEDVEHICLES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,EditParkedVehicleViewModel model)
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

        ParkedVehicle? parkedVehicle = _context.ParkedVehicle.FirstOrDefault(p => p.Id ==id);
        if (parkedVehicle == null) return NotFound(); 
        
        parkedVehicle.RegistrationNumber = model.RegistrationNumber;
        parkedVehicle.VehicleBrand = model.VehicleBrand;
        parkedVehicle.VehicleModel = model.VehicleModel;
        parkedVehicle.VehicleType = model.VehicleType;
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
        
        return View(parkedVehicle);
    }

    // GET: PARKEDVEHICLES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicle = await _context.ParkedVehicle
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
        var vehicle = await _context.ParkedVehicle.FindAsync(id);
        
        if (vehicle == null)
        {
            return NotFound();
        }

        var receipt = new ReceiptViewModel
        {
            Id = vehicle.Id,
            RegistrationNumber = vehicle.RegistrationNumber,
            VehicleType = vehicle.VehicleType,
            VehicleBrand = vehicle.VehicleBrand,
            VehicleModel = vehicle.VehicleModel,
            Color = vehicle.Color,
            Wheels = vehicle.Wheels,
            ArrivalTime = vehicle.ArrivalTime, 
            DepartureTime = DateTime.Now
        };

        _context.ParkedVehicle.Remove(vehicle);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Successfully check out {vehicle} ";
        return View("Receipt", receipt);
    }

    private bool ParkedVehicleExists(int? id)
    {
        return _context.ParkedVehicle.Any(e => e.Id == id);
    }
    
    // GET: Statistics
    public async Task<IActionResult> Statistics()
    {
        var model = new StatisticsParkedVehicleViewModel()
        {
            
        };
        return View(model);
    }
}