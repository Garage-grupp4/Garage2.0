using Garage2._0.Models.ViewModels;
using Garage2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Garage2._0.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Garage2._0.Data;

namespace Garage2._0.Controllers;

[Authorize(Roles = Roles.ADMIN)]
public class ApplicationUserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly GarageContext _context;

    public ApplicationUserController(UserManager<ApplicationUser> userManager, GarageContext context)
    {
        _userManager = userManager;
        _context = context;

    }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(u =>
                    u.Email!.Contains(s) ||
                    u.FirstName.Contains(s) ||
                    u.LastName.Contains(s) ||
                    u.PersonNumber.Contains(s)
                    );
        }

        // Projicera i SQL - en  query
        var raw = await query
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PersonNumber,
                VehicleCount = _context.Vehicles.Count(v => v.ApplicationUserId == u.Id),
                ActiveSessions = _context.Vehicles
                    .Where(v => v.ApplicationUserId == u.Id)
                    .SelectMany(v => v.ParkingSessions)
                    .Where(p => p.DepartureTime == null)
                    .Select(p => new { p.ArrivalTime, p.HourlyRateForParking })
                    .ToList(),
                Role = (from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == u.Id
                        select r.Name).FirstOrDefault() ?? ""
            })
        .ToListAsync();

        // Räkna kostnad i C# (SQLite-säkert)
        var now = DateTime.Now;
        var members = raw.Select(u => new MemberListItemViewModel
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email ?? "",
            PersonNumber = u.PersonNumber,
            VehicleCount = u.VehicleCount,
            Role = u.Role,
            ActiveParkingsCost = u.ActiveSessions
                .Sum(s => (decimal)(now - s.ArrivalTime).TotalHours * s.HourlyRateForParking)
        });

        return View(new MemberSearchViewModel
        {
            Search = search,
            Members = members
        });
    }

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id)) return NotFound();

        var raw = await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PersonNumber,
                Role = (from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == u.Id
                        select r.Name).FirstOrDefault() ?? "",
                Vehicles = _context.Vehicles
                    .Where(v => v.ApplicationUserId == u.Id)
                    .Select(v => new
                    {
                        v.Id,
                        v.RegistrationNumber,
                        v.VehicleBrand,
                        v.VehicleModel,
                        v.Color,
                        VehicleTypeName = v.VehicleType != null ? v.VehicleType.Name : null,
                        ActiveSession = v.ParkingSessions
                            .Where(p => p.DepartureTime == null)
                            .Select(p => new { p.ArrivalTime, p.HourlyRateForParking })
                            .FirstOrDefault()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (raw == null) return NotFound();

        var now = DateTime.Now;
        var vm = new MemberDetailsViewModel
        {
            Id = raw.Id,
            FirstName = raw.FirstName,
            LastName = raw.LastName,
            Email = raw.Email ?? "",
            PersonNumber = raw.PersonNumber,
            Role = raw.Role,
            Vehicles = raw.Vehicles.Select(v => new MemberVehicleViewModel
            {
                Id = v.Id,
                RegistrationNumber = v.RegistrationNumber,
                Brand = v.VehicleBrand,
                Model = v.VehicleModel,
                Color = v.Color,
                VehicleType = v.VehicleTypeName,
                IsCurrentlyParked = v.ActiveSession != null,
                CurrentCost = v.ActiveSession == null
                    ? null
                    : (decimal)(now - v.ActiveSession.ArrivalTime).TotalHours
                        * v.ActiveSession.HourlyRateForParking
            }).ToList()
        };

        return View(vm);
    }

    public async Task<List<ApplicationUserViewModel>> GetUserViewModels()
    {
        var users = new List<ApplicationUserViewModel>();

        var applicationUsers = await _userManager.Users.ToListAsync();
        foreach (var u in applicationUsers)
        {
            var result = await _userManager.GetRolesAsync(u);
            var role = result.FirstOrDefault();
            users.Add(new ApplicationUserViewModel
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PersonNumber = u.PersonNumber,
                Role = role
            });
        }

        return users;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminToggle(string? email)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Email == email);
        if (user == null) return NotFound();
        var result = await _userManager.GetRolesAsync(user);
        bool isAdmin = result.Contains(Roles.ADMIN);

        var newRole = isAdmin ? Roles.MEMBER : Roles.ADMIN;
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, newRole);

        var users = await GetUserViewModels();
        return RedirectToAction(nameof(Index));
    }
}
