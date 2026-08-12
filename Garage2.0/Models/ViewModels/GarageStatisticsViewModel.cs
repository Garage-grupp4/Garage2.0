using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels;

public class GarageStatisticsViewModel
{
    public int FreeGarageSpaces = 0;
    public int OccupiedGarageSpaces = 0;
    
    [Display(Name = "Out of order Garage Spaces")]
    public int OutOfOrderGarageSpaces = 0;
    public IEnumerable<KeyValuePair<string, int>> VehiclesCountPerGarage;
}