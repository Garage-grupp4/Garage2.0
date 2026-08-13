using Garage2._0.Models.ViewModels;
using Garage2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Garage2._0.Constants;
using Garage2._0.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Controllers;

[Authorize(Roles = Roles.ADMIN)]
public class GarageStatisticsController : Controller
{
    private readonly GarageContext _context;

    public GarageStatisticsController(
        GarageContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        
   //     var vehicles = _context.Vehicles.AsQueryable();
  //      var vehicleCounts = vehicles.CountBy(v => v.VehicleType.Name);

        var counts = await _context.Vehicles
            .GroupBy(v => v.VehicleType.Name)
            .Select(g => new KeyValuePair<string, int>(
                g.Key, 
                g.Count()
            ))
            .ToListAsync();
        
        var viewModel = new GarageStatisticsViewModel()
        {
            FreeGarageSpaces = _context.ParkingSpots.Count(p => !p.IsOutOfService &&
                p.ParkingSessions.All(ps => ps.DepartureTime != null)),
            OccupiedGarageSpaces = _context.ParkingSpots.Count(p => p.ParkingSessions.Any(ps => ps.DepartureTime == null)),
            OutOfOrderGarageSpaces = _context.ParkingSpots.Count(p => p.IsOutOfService),
            VehiclesCountPerGarage = counts
            
        };
        return View(viewModel);
    }
    
}