using System.ComponentModel.DataAnnotations;

namespace Web.Models {
    /// <summary>
    /// The ExternalLoginConfirmationViewModel class
    /// </summary>
    public class ExternalLoginConfirmationViewModel {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    } // class

    /// <summary>
    /// The ManageUserViewModel class
    /// </summary>
    public class ManageUserViewModel {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    } // class

    /// <summary>
    /// THe RegisterViewModel class
    /// </summary>
    public class RegisterViewModel {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Product key")]
        public string ProductKey { get; set; }
    } // class
} // namespace
