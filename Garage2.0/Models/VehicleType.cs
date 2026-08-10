using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public class VehicleType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn krävs.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Namn måste vara 1-50 tecken")]
        [Display(Name = "Fordonstyp")]
        public required string Name { get; set; }

        [Display(Name = "Fordon")]
        public ICollection<Vehicle> Vehicles { get; set; } = [];
    }
}
