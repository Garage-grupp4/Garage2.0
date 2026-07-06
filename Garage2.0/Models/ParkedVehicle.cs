using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models;

public class ParkedVehicle
{
    public int Id { get; set; }

    [Display(Name = "Registration Number")]
    public required string registrationNumber { get; set; }

    [Display(Name = "Arrival Time")]
    [DisplayFormat(DataFormatString = "{0:yy-MM-dd HH:mm}")]
    public required DateTime? arrivalTime { get; set; }

    [Display(Name = "Vehicle Type")]
    public required VehicleType vehicleType { get; set; }

    [Display(Name = "Model")]
    public required string vehicleModel { get; set; }

    [Display(Name = "Brand")]
    public required string vehicleBrand { get; set; }

    [Display(Name = "Departure Time")]
    public DateTime? departureTime { get; set; }

    [Display(Name = "Color")]
    public string color { get; set; } = "";

    [Display(Name = "Wheels")]
    [Range(0, 64)]
    public required int wheels { get; set; }
}
