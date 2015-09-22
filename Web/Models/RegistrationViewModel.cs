using System.ComponentModel.DataAnnotations;

namespace Web.Models {
    /// <summary>
    /// The RegistrationViewModel class
    /// </summary>
    public class RegistrationViewModel : UserCredentials {
        public string ProductKey { get; set; }
    } // class

    /// <summary>
    /// The UserCredentials class
    /// </summary>
    public abstract partial class UserCredentials {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    } // class
} // namespace