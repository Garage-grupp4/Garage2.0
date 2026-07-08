namespace Garage2._0.Models.ViewModels;

public class StatisticsParkedVehicleViewModel
{
    public IEnumerable<KeyValuePair<VehicleType, int>> VehicleCount { get; set; }
    
    public int HowManyWheels { get; init; }
    
    public decimal TotalCost { get; init; }
}