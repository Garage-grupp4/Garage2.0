
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage2._0.Models;
using Garage2._0.Models.ViewModels;

public class ParkedVehiclesController : Controller
{
    private readonly _1 _context;

    public ParkedVehiclesController(_1 context)
    {
        _context = context;
    }

    // GET: PARKEDVEHICLES
    public async Task<IActionResult> Index()    
    {
        var vehicles = await _context.ParkedVehicle.ToListAsync();

        var viewModel = vehicles.Select(v => new VehicleOverViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            VehicleType = v.VehicleType,
            ArrivalTime = v.ArrivalTime ?? DateTime.Now
        }).ToList();

        return View(viewModel);
    }

    // GET: PARKEDVEHICLES/Details/5
    public async Task<IActionResult> Details(int? id)
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
            ArrivalTime = model.ArrivalTime,
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

        var parkedvehicle = await _context.ParkedVehicle
            .FirstOrDefaultAsync(m => m.Id == id);
        if (parkedvehicle == null)
        {
            return NotFound();
        }

        return View(parkedvehicle);
    }

    // POST: PARKEDVEHICLES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var parkedvehicle = await _context.ParkedVehicle.FindAsync(id);
        if (parkedvehicle != null)
        {
            _context.ParkedVehicle.Remove(parkedvehicle);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ParkedVehicleExists(int? id)
    {
        return _context.ParkedVehicle.Any(e => e.Id == id);
    }

}
