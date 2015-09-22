using System;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Infrastructure;
using Service.Interfaces;
using Core.Domains;
using System.Web.Security;
using Core;

namespace Web.Controllers {
    /// <summary>
    /// The account controller
    /// </summary>
    public class AccountController : BaseController {

        #region vars

        private readonly IUserService _userService;
        private readonly IUnitService _unitService;
        private readonly ISettingsService _settingsService;

        #endregion

        #region ctor
        
        /// <summary>
        /// Constructor for the AccountController setup for dependency injection
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="unitService"></param>
        /// <param name="settingsService"></param>
        public AccountController( IUserService userService, IUnitService unitService, ISettingsService settingsService ) {
            _userService = userService;
            _unitService = unitService;
            _settingsService = settingsService;
        }

        /// <summary>
        /// Loads the login page
        /// GET
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login( string returnUrl ) {
            ViewBag.ReturnUrl = returnUrl;
            return View( new LoginViewModel() );
        }

        /// <summary>
        /// Attemps to log the user into the application
        /// POST
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(  LoginViewModel model, string returnUrl ) {
            try {
                if ( _userService.ValidateLogin( model.Username, model.Password ) ) {
                    var user = _userService.GetByUsername( model.Username );

                    if ( user == null ) {
                        return View( model );
                    }

                    SaveUserState( user, model.RememberMe );
                    if ( user.Role == Core.Role.Admin ) {
                        return RedirectToAction( "AdminDashboard", "Admin" );
                    }
                    return RedirectToAction( "Dashboard", "Home" );
                }
                return View( model );
            } catch ( Exception ex ) {
                // TODO: return login error to view
                return View();
            }
        }

        /// <summary>
        /// Loads the register page
        /// GET
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register() {
            return View( new RegistrationViewModel() );
        }

        /// <summary>
        /// Attempts to register a new user
        /// POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register( RegistrationViewModel model ) {
            try {
                // validate unique username
                if ( !_userService.ValidateUsername( model.Username ) ) { 
                    return View( model ); // TODO: return error
                }
                // validate product key
                if ( _unitService.ValidateProductKey( model.ProductKey ) != -1 ) {
                    return View( model ); // TODO: return error
                }

                var unit = new Unit {
                    ProductKey = model.ProductKey
                };

                _unitService.Insert( unit );

                var newUser = new User {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Role = Core.Role.User,
                    UnitId = unit.Id
                };

                _userService.Insert( newUser );

                SaveUserState( newUser, false );
                return RedirectToAction( "Dashboard", "Home" );
            } catch ( Exception ex ) {
                return View( model );
            }
        }

        /// <summary>
        /// Load the settings page with the user's current settings
        /// GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult Settings() {
            try {
                var user = _userService.GetUserById( UserId );
                return View( new SettingsViewModel( user ) );
            } catch ( Exception ex ) {
                return View();
            }
        }

        /// <summary>
        /// Update the user's settings
        /// POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult SaveSettings( SettingsViewModel model ) {
            try {
                _settingsService.Insert( model.UnitSettings );
                
                var user = _userService.GetUserById( UserId );
                if ( !user.Unit.SettingsId.HasValue ) {
                    user.Unit.SettingsId = model.UnitSettings.Id;
                }
                if ( !string.IsNullOrEmpty( model.Password ) && model.Password == model.ConfirmPassword && _userService.ValidateLogin( model.Username, model.Password ) ) {
                    _userService.ChangePassword( user, model.Password );
                } else {
                    _userService.Insert( user );
                }
                return RedirectToAction( "Dashboard", "Home" );
            } catch (Exception ex) {
                return View();
            }
        }

        /// <summary>
        /// Sign the user out and return to the login page
        /// </summary>
        /// <returns></returns>
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult Signout() {
            FormsAuthentication.SignOut();
            return RedirectToAction( "Login", "Account" );
        }

        /// <summary>
        /// Save the user state cookie
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rememberMe"></param>
        private void SaveUserState( User user, bool rememberMe ) {
            var currentDateTime = DateTime.UtcNow.ToLocalTime();
            var userData = new UserDataModel { Id = user.Id, Role = user.Role };
            var authTicket = new FormsAuthenticationTicket(
                                    version: 1,
                                    name: user.Username,
                                    issueDate: currentDateTime,
                                    expiration: currentDateTime.AddMinutes( 30 ),
                                    isPersistent: rememberMe,
                                    userData: JsonManager.Serialize( userData ),
                                    cookiePath: FormsAuthentication.FormsCookiePath );

            var encryptedTicket = FormsAuthentication.Encrypt( ticket: authTicket );
            var httpCookie = new HttpCookie( FormsAuthentication.FormsCookieName, encryptedTicket ) {
                HttpOnly = true
            };

            if ( authTicket.IsPersistent ) {
                httpCookie.Expires = authTicket.Expiration;
            }

            httpCookie.Path = FormsAuthentication.FormsCookiePath;
            if ( FormsAuthentication.CookieDomain != null )
                httpCookie.Domain = FormsAuthentication.CookieDomain;

            HttpContext.Response.Cookies.Add( httpCookie );
        }

        #endregion
    } // class
} // namespace