using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels
{
    public class VehicleOverViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Display(Name = "Vehicle Type")]
        public VehicleType? VehicleType { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime? ArrivalTime { get; set; }

        [Display(Name = "Parked Duration")]
        public string ParkedDuration => ArrivalTime.HasValue ? (DateTime.Now - ArrivalTime.Value).ToString(@"d\:hh\:mm") : string.Empty;
    }
}
