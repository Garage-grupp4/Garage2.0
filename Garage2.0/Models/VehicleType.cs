using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public enum VehicleType
    {
        [Display(Name = "Car")]
        Car,

        [Display(Name = "Bus")]
        Bus,

        [Display(Name = "Truck")]
        Truck,

        [Display(Name = "Motorcycle")]
        Motorcycle,
    }
}
