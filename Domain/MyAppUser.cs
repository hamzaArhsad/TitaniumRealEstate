using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class MyAppUser:IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 20 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(20, MinimumLength =3, ErrorMessage = "First name must be between 3 and 20 characters")]
        public string LastName { get; set; }
    }
}
