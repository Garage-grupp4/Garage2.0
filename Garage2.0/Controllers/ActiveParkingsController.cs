using Garage2._0.Data;
using Garage2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Controllers;

public class ActiveParkingsController : Controller
{
    private readonly GarageContext _context;
    public ActiveParkingsController(GarageContext context)
    {
        _context = context;
    }

    // GET: ActiveParkings
    public async Task<IActionResult> Index(string? search, int? vehicleTypeId)
    {
        // Base query: only active parkings (DepartureTime == null)
        var query = _context.Set<Models.ParkingSession>()
            .Where(ps => ps.DepartureTime == null);

        // TASK-08.5: Search by registration number
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(ps =>
                ps.Vehicle.RegistrationNumber.Contains(search.Trim()));
        }

        // TASK-08.6: Filter by vehicle type
        if (vehicleTypeId.HasValue)
        {
            query = query.Where(ps =>
                ps.Vehicle.VehicleTypeId == vehicleTypeId.Value);
        }

        // TASK-08.2: Project to ViewModel (single SQL query, no N+1)
        var activeParkings = await query
            .Select(ps => new ActiveParkingViewModel
            {
                OwnerName = ps.Vehicle.ApplicationUser.FirstName + " " +
                            ps.Vehicle.ApplicationUser.LastName,
                VehicleType = ps.Vehicle.VehicleType!.Name,
                RegistrationNumber = ps.Vehicle.RegistrationNumber,
                SpotNumber = ps.ParkingSpot.Number,
                Location = ps.ParkingSpot.Location,
                ArrivalTime = ps.ArrivalTime,
                Duration = FormatDuration(DateTime.Now - ps.ArrivalTime)
            })
            .ToListAsync();

        // Populate filter dropdown
        ViewBag.VehicleTypes = new SelectList(
            await _context.VehicleTypes.OrderBy(vt => vt.Name).ToListAsync(),
            "Id", "Name", vehicleTypeId);

        // Preserve search/filter values in form
        ViewBag.CurrentSearch = search;
        ViewBag.CurrentVehicleType = vehicleTypeId;

        return View(activeParkings);
    }

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
            return $"{(int)duration.TotalDays}d {duration.Hours}h {duration.Minutes}m";

        return $"{duration.Hours}h {duration.Minutes}m";
    }
}
