using Garage2._0.Models.ViewModels;
using Garage2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Garage2._0.Controllers;

public class ApplicationUserController : Controller
{
    private UserManager<ApplicationUser> _userManager;

    public ApplicationUserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public IActionResult Index()
    { 


        var users = _userManager.Users.ToArray();
        return View(users);
    }
}