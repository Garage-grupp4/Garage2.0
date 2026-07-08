
using Garage2._0.Models;
using Garage2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Drawing;

public class ParkedVehiclesController : Controller  //viewmodel för att visa en lista med parkerade fordon, med möjlighet att filtrera efter registreringsnummer och fordonstyp.
{
    private readonly _1 _context;

    public ParkedVehiclesController(_1 context)
    {
        _context = context;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index(string license, VehicleType? type)
    {
        IQueryable <ParkedVehicle>vehicles = _context.ParkedVehicle; // Query the database for all parked vehicles. Removed select and var to avoid unnecessary data retrieval from the database.

        // Save search terms to populate html page
        ViewData["license"] = license;
        ViewData["type"] = type;

        // Filter with search terms
        if (!string.IsNullOrEmpty(license))
            vehicles = vehicles.Where(v => v.RegistrationNumber.ToUpper().StartsWith(license.ToUpper())); // changed to startwith to make it more user friendly kanske använda Equals instead of ToUpper() for exact match, but then it would be case sensitive. Could use ToLower() instead of ToUpper() for case insensitive match.
        if (type != null)
            vehicles = vehicles.Where(v => v.VehicleType == type);

        var viewModel = (await vehicles.ToListAsync()).Select(v => new VehicleOverViewModel // man ska inte ha select före ToListAsync() 
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            VehicleType = v.VehicleType,
            ArrivalTime = v.ArrivalTime ?? DateTime.Now
        
        }).ToList();

        return View(viewModel);
    }

    //GET: PARKEDVEHICLES/Details/5
    public async Task<IActionResult> Details(int? id) //kan göra lite snyggar här
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
            ArrivalTime = parkedvehicle.ArrivalTime ?? DateTime.MinValue,
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
    public async Task<IActionResult> Edit(EditParkedVehicleViewModel model)
    {

        // Validate if unique number or return model error
        if (model.OriginalRegistrationNumber != model.RegistrationNumber && !await IsRegistrationNumberUnique(model.RegistrationNumber))
        {
            string warning = $"Registration number {model.RegistrationNumber} is already in use.";
            ModelState.AddModelError(nameof(model.RegistrationNumber), warning);
            TempData["Warning"] = warning;
            return View(model);
        }
        
        // Generate newVehicle
        ParkedVehicle newParkedVehicle = new ParkedVehicle()
        {
            ArrivalTime = model.ArrivalTime, //ToDo : Check if this is correct it is manipulable.
            RegistrationNumber = model.RegistrationNumber,
            VehicleBrand = model.VehicleBrand,
            VehicleModel = model.VehicleModel,
            VehicleType = model.VehicleType,
            Color = model.Color,
            Wheels = model.Wheels,
            Id = model.Id,
        };
        
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(newParkedVehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(newParkedVehicle.Id))
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
            TempData["Success"] = $"Successfully edited Vehicle {newParkedVehicle}";
            return RedirectToAction(nameof(Index));
        }
        
        return View(newParkedVehicle);
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
            ArrivalTime = vehicle.ArrivalTime ?? DateTime.Now,
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

}
