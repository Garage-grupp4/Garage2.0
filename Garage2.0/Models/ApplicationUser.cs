using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Garage2._0.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^\d{8}-\d{4}$",
        ErrorMessage = "Personal Number must be in format YYYYMMDD-XXXX"
        )]
    [StringLength(13, MinimumLength = 13)]
    [Display(Name = "Personal Number")]
    public string PersonNumber { get; set; } = string.Empty;


   public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
   
   public DateTime PersonNumberToDateTime()  
   {  
       if (PersonNumber.Length < 8) throw  new Exception("Person number dosn't have enough data");  
       int year, month, day;  
       year = Convert.ToInt32(PersonNumber.Substring(0, 4));  
       month = Convert.ToInt32(PersonNumber.Substring(4, 2));  
       day = Convert.ToInt32(PersonNumber.Substring(6, 2));  
       return new DateTime(year, month, day);  
   }
}
