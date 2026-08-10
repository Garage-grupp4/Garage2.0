using Garage2._0.Models.ViewModels;
using Garage2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Garage2._0.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Controllers;

[Authorize(Roles = Roles.ADMIN)]
public class GarageStatisticsController : Controller
{
    public IActionResult Index()
    {
        // TODO use real stats
        var vehicleCount = new List<KeyValuePair<string, int>>();
        vehicleCount.Add(new KeyValuePair<string, int>("Car", 2));
        vehicleCount.Add(new KeyValuePair<string, int>("Truck", 1));
        var viewModel = new GarageStatisticsViewModel()
        {
            FreeGarageSpaces = 4,
            OccupiedGarageSpaces = 3,
            OutOfOrderGarageSpaces = 2,
            VehiclesCountPerGarage = vehicleCount
            
        };
        return View(viewModel);
    }
    
}