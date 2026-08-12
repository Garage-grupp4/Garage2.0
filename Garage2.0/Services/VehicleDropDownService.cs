using Garage2._0.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Services
{
    public class VehicleDropDownService : IVehicleDropDownService
    {
        private readonly GarageContext context;

        public VehicleDropDownService(GarageContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetVehicleSelectListAsync(string userId)
        {
            var vehicleTypes = await context.Vehicles
                .Where(v => v.ApplicationUserId == userId)
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
