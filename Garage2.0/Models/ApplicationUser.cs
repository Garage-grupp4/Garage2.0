using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Garage2._0.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [Display(Name = "Förnamn")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    [Display(Name = "Efternamn")]
    public string LastName { get; set; } = string.Empty;

    // TODO(#39): Custom validation för PersonNumber-format (YYYYMMDD-XXXX)
    // TODO(#40): Unikt index på PersonNumber i DbContextalidation
    [Required]
    [StringLength(13, MinimumLength = 13)]
    [Display(Name = "Personnummer")]
    public string PersonNumber { get; set; } = string.Empty;
}
