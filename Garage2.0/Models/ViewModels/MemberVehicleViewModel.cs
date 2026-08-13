using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels;

public class MemberVehicleViewModel
{
    public int Id { get; set; }

    [Display(Name = "Registration number")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Display(Name = "Brand")]
    public string Brand { get; set; } = string.Empty;

    [Display(Name = "Model")]
    public string Model { get; set; } = string.Empty;

    [Display(Name = "Color")]
    public string? Color { get; set; }

    [Display(Name = "Type")]
    public string? VehicleType { get; set; }

    [Display(Name = "Currently parked")]
    public bool IsCurrentlyParked { get; set; }

    [Display(Name = "Current cost")]
    public decimal? CurrentCost { get; set; }
}
