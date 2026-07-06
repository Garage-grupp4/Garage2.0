using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Garage2._0.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }

        public required string registrationNumber { get; set; }

        public required DateTime? arrivalTime { get; set; }

        public required VehicleType vehicleType { get; set; }

        public required string vehicleModel { get; set; }

        public required string vehicleBrand { get; set; }

        public DateTime? departureTime { get; set; }

        public short color { get; set; }

        [Range(0, 64)]
        public required int wheels { get; set; }
    }
}
