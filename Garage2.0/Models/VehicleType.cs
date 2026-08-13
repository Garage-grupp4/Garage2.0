using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public class VehicleType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be 1-50 characters.")]
        [Display(Name = "Vehicle Type")]
        public required string Name { get; set; }

        [Display(Name = "Vehicles")]
        public ICollection<Vehicle> Vehicles { get; set; } = [];
    }
}
