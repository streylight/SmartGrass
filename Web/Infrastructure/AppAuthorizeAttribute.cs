using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Core;
using Core.Domains;
using Newtonsoft.Json;

namespace Web.Infrastructure {
    /// <summary>
    /// Web Authorize attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AppAuthorizeAttribute : AuthorizeAttribute {
        /// <summary>
        /// The user roles
        /// </summary>
        private readonly Role[] _userRoles;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="userRoles">The user types</param>
        public AppAuthorizeAttribute(params Role[] userRoles) {
            _userRoles = userRoles;
        }

        /// <summary>
        /// Checks to see if the user is authenticated and has the correct role to access a particular view
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <returns>boolean</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)  {
            if (httpContext == null)
                return false;

            if (httpContext.Session == null)
                return false;

            // Check if the user is authenticated.
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            // Load the user type
            //var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            //if (authCookie != null)
            //{
            //    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    var data = authTicket.UserData;
            //}

            var id = (FormsIdentity) httpContext.User.Identity;
            var data = JsonConvert.DeserializeObject<UserDataModel>(id.Ticket.UserData);

            //var userData = id.Ticket.UserData.Split(',')[0]; // first index has role
            var role = (Role)Enum.Parse(typeof(Role), data.Role.ToString());
            
            return !_userRoles.Any() || _userRoles.Contains(role);
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        /// <exception cref="System.ArgumentNullException">filterContext</exception>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)  {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            filterContext.Result = new ViewResult { ViewName = "NotAuthorized" };
        }
    } // class
}