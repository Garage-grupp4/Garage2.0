using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage2._0.Models
{
    public class ParkVehicleViewModel
    {
        public int VehicleId { get; set; }
        public int ParkingSpotId { get; set; }


        public IEnumerable<SelectListItem> AvailableVehicles { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> AvailableParkingSpots { get; set; } = new List<SelectListItem>();

    }
}
