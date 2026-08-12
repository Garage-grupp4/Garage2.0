using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Spot number is required.")]
        [Range(1, 9999, ErrorMessage = "Spot number must be between 1 and 9999.")]
        [Display(Name = "Spot Number")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = " Location cannot exceed 100 characters.")]
        [Display(Name = "Location")]
        public required string Location { get; set; }

        [Display(Name = "Out of Service")]
        public bool IsOutOfService { get; set; }

        public ICollection<ParkingSession> ParkingSessions { get; set; } = [];
    }
}
