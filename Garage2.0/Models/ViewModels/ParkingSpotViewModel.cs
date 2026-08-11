namespace Garage2._0.Models.ViewModels;

public class ParkingSpotViewModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public required string Location { get; set; }
    public bool IsOutOfService { get; set; }
    public required string Status { get; set; }
}
