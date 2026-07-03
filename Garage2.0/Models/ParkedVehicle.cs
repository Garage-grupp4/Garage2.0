using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Models
{
    public class ParkedVehicle
    {
        
        public int Id { get; set; }
        
        public required string RegistrationNumber { get; set; }
        
        public required DateTime? ArrivalTime { get; set; }

        public required VehicleType VehicleType { get; set; }
        
        public required string VehicleModel { get; set; }

        public required string VehicleBrand { get; set; }

        public DateTime? DepartureTime { get; set; }

        public string? Color { get; set; }

        [Range(0, 64)]
        public required int Wheels { get; set; }
    }
}
