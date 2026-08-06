using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public class VehicleType
    {
        public  int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = [];
    }
}
