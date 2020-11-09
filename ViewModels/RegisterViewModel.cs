using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "Too short", MinimumLength = 6)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Too short", MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "Too short", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        public string PasswordConfirm { get; set; }
    }
}
