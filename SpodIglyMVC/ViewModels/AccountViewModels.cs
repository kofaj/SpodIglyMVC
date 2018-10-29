
using System.ComponentModel.DataAnnotations;

namespace SpodIglyMVC.ViewModels
{
    public class LoginViewModels
    {
        [Required(ErrorMessage = "Musisz wprowadzić e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamietaj mnie")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required()]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}