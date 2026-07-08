namespace Garage2._0.Helpers;

public static class PriceHelper
{
    private const decimal pricePerMinute = 0.5m;
    public static decimal CalculatePrice(TimeSpan time)
    {
        return (decimal)time.TotalMinutes * pricePerMinute;
    }

    public static decimal CalulatePrice(DateTime arrivalTime)
    {
        return CalculatePrice(DateTime.Now - arrivalTime);
    }
}