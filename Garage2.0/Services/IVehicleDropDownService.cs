using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2._0.Services
{
    public interface IVehicleDropDownService
    {
        Task<IEnumerable<SelectListItem>> GetVehicleSelectListAsync(string userId);
    }
}