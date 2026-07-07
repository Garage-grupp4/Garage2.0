namespace Garage2._0.Models
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }

        public string? RegistrationNumber { get; set; }

        public VehicleType VehicleType { get; set; }

        public string? VehicleBrand { get; set; }

        public string? VehicleModel { get; set; }

        public string? Color { get; set; }

        public int Wheels { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime DepartureTime { get; set; }

        public TimeSpan TotalTimeParked => DepartureTime - ArrivalTime;

        private const decimal PricePerMinute = 0.5m; // Example price per minute

        public decimal TotalPrice => (decimal)TotalTimeParked.TotalMinutes * PricePerMinute;
    }
}
