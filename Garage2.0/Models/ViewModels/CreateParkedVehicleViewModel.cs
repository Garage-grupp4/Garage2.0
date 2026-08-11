using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;

namespace Garage2._0.Models.ViewModels;

public class CreateParkedVehicleViewModel
{
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Registration number of invalid size")]
    [Display(Name = "Registration Number")]
    public required string RegistrationNumber { get; set; } //CuStom validation. RE USE ON EDIT PARKED VEHICLE VIEW MODEL

    [Display(Name = "Type")]
    public required VehicleType VehicleType { get; set; }

    [StringLength(20, ErrorMessage = "Name length can't be more than 20.")]
    [Display(Name = "Model")]
    public required string VehicleModel { get; set; }

    [StringLength(20, ErrorMessage = "Name length can't be more than 20.")]
    [Display(Name = "Brand")]
    public required string VehicleBrand { get; set; }

    [Display(Name = "Vehicle Color")]
    public string Color { get; set; }

    [Display(Name = "Number Of Wheels")]
    [Range(0, 64, ErrorMessage = "Can't have that amount of wheels")]
    public required int Wheels { get; set; }
}