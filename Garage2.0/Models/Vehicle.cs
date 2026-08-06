using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Models;

public class Vehicle
{
        
    public int Id { get; set; }

    [StringLength(10, MinimumLength = 1, ErrorMessage = "Registration number of invalid size")]
    [Display(Name = "Registration Number")]
    public required string RegistrationNumber { get; set; } //Custom validation

    [Display(Name = "Model")]
    public required string VehicleModel { get; set; }

    [Display(Name = "Brand")]
    public required string VehicleBrand { get; set; }

    public string? Color { get; set; }

    [Range(0, 64)] 
    public required int Wheels { get; set; }

    public override string ToString()
    {
        return $"{VehicleType} {RegistrationNumber}";
    }

    [Display(Name = "Vehicle Type")]
    public required VehicleType VehicleType { get; set; }

    public int VehicleTypeId { get; set; } 

}