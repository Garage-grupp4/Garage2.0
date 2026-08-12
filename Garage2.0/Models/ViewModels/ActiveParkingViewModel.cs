using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels;

public class ActiveParkingViewModel
{
    [Display(Name = "Owner")]
    public string OwnerName { get; set; } = string.Empty;

    [Display(Name = "Vehicle Type")]
    public string VehicleType { get; set; } = string.Empty;

    [Display(Name = "Registration Number")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Display(Name = "Spot Number")]
    public int SpotNumber { get; set; }

    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;

    [Display(Name = "Arrival Time")]
    [DisplayFormat(DataFormatString = "{0:yy-MM-dd HH:mm}")]
    public DateTime ArrivalTime { get; set; }

    [Display(Name = "Duration")]
    public string Duration { get; set; } = string.Empty;
}
