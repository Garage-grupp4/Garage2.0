using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2._0.Data;
using Garage2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Garage2._0.Constants;
using Garage2._0.Models.ViewModels;

namespace Garage2._0.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ParkingSpotsController : Controller
    {
        private readonly GarageContext _context;

        public ParkingSpotsController(GarageContext context)
        {
            _context = context;
        }

        // GET: ParkingSpots
        public async Task<IActionResult> Index()
        {
            var spots = await _context.ParkingSpots
                .Select(ps => new ParkingSpotViewModel
                {
                    Id = ps.Id,
                    Number = ps.Number,
                    Location = ps.Location,
                    Status = ps.IsOutOfService ? "Out of Service" : ps.ParkingSessions
                        .Any(session => session.DepartureTime == null)
                        ? "Occupied" : "Available"
                }
                        ).ToListAsync();
            return View(spots);
        }

        // GET: ParkingSpots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // GET: ParkingSpots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingSpots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Location,IsOutOfService")] ParkingSpot parkingSpot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingSpot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingSpot);
        }

        // GET: ParkingSpots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots.FindAsync(id);
            if (parkingSpot == null)
            {
                return NotFound();
            }
            return View(parkingSpot);
        }

        // POST: ParkingSpots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Location,IsOutOfService")] ParkingSpot parkingSpot)
        {
            if (id != parkingSpot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingSpot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingSpotExists(parkingSpot.Id))
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
            return View(parkingSpot);
        }

        // GET: ParkingSpots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // POST: ParkingSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSpot = await _context.ParkingSpots
                .Include(ps => ps.ParkingSessions)
                .FirstOrDefaultAsync(ps => ps.Id == id);

            if (parkingSpot is null) return NotFound();

            if (parkingSpot.ParkingSessions.Count > 0)
            {
                TempData["Error"] = $"Kan inte ta bort plats {parkingSpot.Number} — {parkingSpot.ParkingSessions.Count} parkering(ar) i historiken. Markera platsen som 'Ur funktion' istället.";
                return RedirectToAction(nameof(Index));
            }
            _context.ParkingSpots.Remove(parkingSpot);

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Plats {parkingSpot.Number} togs bort.";
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSpotExists(int id)
        {
            return _context.ParkingSpots.Any(e => e.Id == id);
        }
    }
}
