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
public class ApplicationUserController : Controller
{
    private UserManager<ApplicationUser> _userManager;

    public ApplicationUserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
        var users = await GetUserViewModels();

        return View(users);
    }

    public async Task<List<ApplicationUserViewModel>> GetUserViewModels()
    {
        var users = new List<ApplicationUserViewModel>();

        var applicationUsers = await _userManager.Users.ToListAsync();
        foreach (var u in applicationUsers)
        {
            var result = await _userManager.GetRolesAsync(u);
            var role = result.FirstOrDefault();
            users.Add(new ApplicationUserViewModel             {
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