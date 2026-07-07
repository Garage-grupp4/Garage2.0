namespace Garage2._0.Models
{
    public class VehicleOverViewModel
    {
        public int Id { get; set; }

        public string RegistrationNumber { get; set; } = string.Empty;

        public VehicleType VehicleType { get; set; }

        public DateTime ArrivalTime { get; set; }

        public string ParkedDuration => (DateTime.Now - ArrivalTime).ToString(@"d\:hh\:mm");
    }
}
