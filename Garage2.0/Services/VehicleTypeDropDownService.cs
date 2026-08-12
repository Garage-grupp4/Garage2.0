using Garage2._0.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Services
{
    public class VehicleTypeDropDownService : IVehicleTypeDropDownService
    {
        private readonly GarageContext context;

        public VehicleTypeDropDownService(GarageContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetVehicleTypesAsync()
        {
            var vehicleTypes = await context.VehicleTypes
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.Name
                })
                .ToListAsync();
            return vehicleTypes;
        }
    }
}
