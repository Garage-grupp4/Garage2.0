namespace Garage2._0.Models.ViewModels;

public class StatisticsParkedVehicleViewModel
{
    public Dictionary<VehicleType,int> VehicleCount { get; set; }
    
    public int HowManyWheels { get; init; }
    
    public int CurrentDept { get; init; }
}