using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage2._0.Models
{
    public class ParkingSession
    {
        public int Id { get; set; }

        public required int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        public required int ParkingSpotId { get; set; }
        public ParkingSpot ParkingSpot { get; set; } = null!;

        [Display(Name = "Arrival time")]
        [DisplayFormat(DataFormatString = "{0:yy-MM-dd HH:mm}")]
        public required DateTime ArrivalTime { get; set; }

        [Display(Name = "Departure time")]
        public DateTime? DepartureTime { get; set; }

        public decimal HourlyRateForParking { get; set; }

        public decimal? TotalCost { get; set; }
    }
}
