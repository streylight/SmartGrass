using System.Web.Mvc;
using System.Web.Security;
using Core;
using Core.Domains;

namespace Web.Infrastructure {
    public class BaseController : Controller {

        protected Role Role {
            get {
                var identity = (FormsIdentity)HttpContext.User.Identity;
                return JsonManager.Deserialize<UserDataModel>(identity.Ticket.UserData).Role;
            }
        }

        protected string Username {
            get { return User.Identity.Name; }
        }

        protected int UserId  {
            get {
                var identity = (FormsIdentity)HttpContext.User.Identity;
                return JsonManager.Deserialize<UserDataModel>(identity.Ticket.UserData).Id;
            }
        }

        /// <summary>
        /// Adds TempData with specified Attention message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Attention(string message) {
            TempData.Add(Alert.Attention, message);
        }

        /// <summary>
        /// Adds TempData with specified Success message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Success(string message)
        {
            TempData.Add(Alert.Success, message);
        }

        /// <summary>
        /// Adds TempData with specified Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Information(string message)
        {
            TempData.Add(Alert.Information, message);
        }

        /// <summary>
        /// Adds TempData with specified Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            TempData.Add(Alert.Error, message);
        }
    } // BaseController

    /// <summary>
    /// Alert
    /// </summary>
    public static class Alert {
        /// <summary>
        /// The success
        /// </summary>
        public const string Success = "success";

        /// <summary>
        /// The attention
        /// </summary>
        public const string Attention = "attention";

        /// <summary>
        /// The error
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// The information
        /// </summary>
        public const string Information = "info";

        /// <summary>
        /// Gets the ALL.
        /// </summary>
        public static string[] All {
            get { return new[] { Success, Attention, Information, Error }; }
        }
    } // Alert
} // namespace