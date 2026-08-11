using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels;

public class ParkingSpotViewModel
{
    public int Id { get; set; }

    [Display(Name = "Spot Number")]
    public int Number { get; set; }

    [Display(Name = "Location")]
    public required string Location { get; set; }

    [Display(Name = "Out of Service")]
    public bool IsOutOfService { get; set; }

    [Display(Name = "Status")]
    public required string Status { get; set; }
}
