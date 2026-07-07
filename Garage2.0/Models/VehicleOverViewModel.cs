using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models
{
    public class VehicleOverViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Parked Duration")]
        public string ParkedDuration => (DateTime.Now - ArrivalTime).ToString(@"d\:hh\:mm");
    }
}
