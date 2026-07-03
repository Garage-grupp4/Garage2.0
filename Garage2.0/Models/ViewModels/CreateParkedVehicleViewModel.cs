using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;

namespace Garage2._0.Models.ViewModels;

public class CreateParkedVehicleViewModel
{
    [Display(Name = "Registration Number")]
    [Remote(action: "VerifyRegistrationNumber",controller: "ParkedVehicles")]
    public required string RegistrationNumber { get; set; }

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
    public required int wheels { get; set; }
}