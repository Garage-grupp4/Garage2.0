namespace Garage2._0.Models.ViewModels
{
  public class ErrorViewModel
  {
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
  }
}
