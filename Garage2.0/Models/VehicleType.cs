using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public enum VehicleType
    {
        [Display(Name = "Bil")]
        Car,

        [Display(Name = "Buss")]
        Bus,

        [Display(Name = "Lastbil")]
        Truck,

        [Display(Name = "Motorcykel")]
        Motorcycle,
    }
}
