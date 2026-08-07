using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        public int Number { get; set; }

        [StringLength(100)]
        public required string Location { get; set; }

        public bool isOutOfService { get; set; } 

        public ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();
    }
}
