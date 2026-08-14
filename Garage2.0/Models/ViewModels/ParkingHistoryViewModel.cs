namespace Garage2._0.Models.ViewModels
{
    public class ParkingHistoryViewModel
    {
        public int ParkingSessionId { get; set; }

        public string RegistrationNumber { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
