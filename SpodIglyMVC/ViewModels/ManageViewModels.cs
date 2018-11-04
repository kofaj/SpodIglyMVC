using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SpodIglyMVC.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SpodIglyMVC.ViewModels
{
    public class ManageCredentialsViewModel
    {
        public bool HasPassword { get; set; }
        public SetPasswordViewModel SetPasswordViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public SpodIglyMVC.Controllers.ManageController. ManageMessageId? Message { get; set; }
        public IList<UserLoginInfo> CurrentLogin { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
        public bool ShowRemoveButton { get; set; }

        public UserData UserData { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}