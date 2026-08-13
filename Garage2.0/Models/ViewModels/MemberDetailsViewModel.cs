using System.ComponentModel.DataAnnotations;

namespace Garage2._0.Models.ViewModels;

public class MemberDetailsViewModel
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Personal Number")]
    public string PersonNumber { get; set; } = string.Empty;

    [Display(Name = "Role")]
    public string Role { get; set; } = string.Empty;

    public List<MemberVehicleViewModel> Vehicles { get; set; } = [];

}
