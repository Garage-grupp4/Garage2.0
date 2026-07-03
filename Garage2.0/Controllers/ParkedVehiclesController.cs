
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
        return View(await _context.ParkedVehicle.ToListAsync());
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

        ParkedVehicle newParkedVehicle = new ParkedVehicle()
        {
            ArrivalTime = DateTime.Now,
            RegistrationNumber = model.RegistrationNumber,
            VehicleBrand = model.VehicleBrand,
            VehicleModel = model.VehicleModel,
            VehicleType = model.VehicleType,
            Color = model.Color,
            Wheels = model.wheels
        };
        if (ModelState.IsValid)
        {
            _context.Add(newParkedVehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View();
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
        return View(parkedvehicle);
    }

    // POST: PARKEDVEHICLES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,registrationNumber,arrivalTime,vehicleType,vehicleModel,vehicleBrand,departureTime,color,wheels")] ParkedVehicle parkedvehicle)
    {
        if (id != parkedvehicle.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(parkedvehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkedVehicleExists(parkedvehicle.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(parkedvehicle);
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
