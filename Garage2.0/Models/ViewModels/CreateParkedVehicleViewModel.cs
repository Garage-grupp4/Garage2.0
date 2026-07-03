using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Garage2._0.Models.ViewModels;

public class CreateParkedVehicleViewModel
{
    [Display(Name = "Registration Number")]
    public required string RegistrationNumber { get; set; }

    [Display(Name = "Type")]
    public required VehicleType VehicleType { get; set; }

    [Display(Name = "Model")]
    public required string VehicleModel { get; set; }

    [Display(Name = "Brand")]
    public required string VehicleBrand { get; set; }
    
    [Display(Name = "Vehicle Color")]
    public string Color { get; set; }

    [Display(Name = "Number Of Wheels")]
    [Range(0, 64)]
    public required int wheels { get; set; }
}