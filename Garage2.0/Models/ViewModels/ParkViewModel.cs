using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2._0.Models.ViewModels
{
    public class ParkViewModel
    {
        public int SelectedVehicleId { get; set; }
        public IEnumerable<SelectListItem> Vehicles { get; set; } = [];
    }
}
