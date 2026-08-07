using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels;

public class ApplicationUserViewModel
{
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PersonNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}