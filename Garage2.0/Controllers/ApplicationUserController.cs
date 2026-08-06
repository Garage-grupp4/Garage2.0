using Garage2._0.Models.ViewModels;
using Garage2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Garage2._0.Controllers;

public class ApplicationUserController : Controller
{
    [Authorize(Roles ="Admin")]
    public IActionResult Index()
    {
        return View();
    }
}