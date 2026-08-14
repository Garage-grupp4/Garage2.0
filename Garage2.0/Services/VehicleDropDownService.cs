using System.Security.Claims;
using Garage2._0.Constants;
using Garage2._0.Data;
using Garage2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace Garage2._0.Services
{
    public class VehicleDropDownService : IVehicleDropDownService
    {
        private readonly GarageContext context;
        private readonly UserManager<ApplicationUser> _userManager;
        public VehicleDropDownService(GarageContext context,UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<SelectListItem>> GetVehicleSelectListAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            
            var vehicleTypes = await context.Vehicles
                .Where(v => roles.Contains(Roles.ADMIN) || v.ApplicationUserId == user.Id)
                .Where(v => v.ParkingSessions.All(p => p.DepartureTime != null))
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.RegistrationNumber
                })
                .ToListAsync();
            return vehicleTypes;
        }
    }
}
