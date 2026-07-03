using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models;

public class ParkedVehicle
{
    public int Id { get; set; }

    [Display(Name = "Registreringsnummer")]
    public required string registrationNumber { get; set; }

    [Display(Name = "Ankomsttid")]
    [DisplayFormat(DataFormatString = "{0:yy-MM-dd HH:mm}")]
    public required DateTime? arrivalTime { get; set; }

    [Display(Name = "Fordonstyp")]
    public required VehicleType vehicleType { get; set; }

    [Display(Name = "Modell")]
    public required string vehicleModel { get; set; }

    [Display(Name = "Märke")]
    public required string vehicleBrand { get; set; }

    [Display(Name = "Utcheckningstid")]
    public DateTime? departureTime { get; set; }

    [Display(Name = "Färg")]
    public string color { get; set; } = "";

    [Display(Name = "Antal hjul")]
    [Range(0, 64)]
    public required int wheels { get; set; }
}
