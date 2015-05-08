using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        private readonly IUserService _userService;
        private readonly IUnitService _unitService;
        private readonly ISettingsService _settingsService;

        public AccountController(IUserService userService, IUnitService unitService, ISettingsService settingsService) {
            _userService = userService;
            _unitService = unitService;
            _settingsService = settingsService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            try {
                if (_userService.ValidateLogin(model.Username, model.Password)) {
                    var user = _userService.GetByUsername(model.Username);

                    if (user == null) return View(model);

                    SaveUserState(user, model.RememberMe);
                    if (user.Role == Core.Role.Admin)
                        return RedirectToAction("AdminDashboard", "Admin");

                    return RedirectToAction("Dashboard", "Home");
                }
                return View(model);
            } catch (Exception ex) {
                // TODO: return error to view
                return View();
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register() {
            return View(new RegistrationViewModel());
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationViewModel model) {
            try {

                if (!_userService.ValidateUsername(model.Username)) return View(model); // TODO: return error
                if (_unitService.ValidateProductKey(model.ProductKey) != -1) return View(model); // TODO: return error

                var unit = new Unit {
                    ProductKey = model.ProductKey
                };

                _unitService.Insert(unit);

                var newUser = new User {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Role = Core.Role.User,
                    UnitId = unit.Id
                };

                _userService.Insert(newUser);

                SaveUserState(newUser, false);
                return RedirectToAction("Dashboard", "Home");
            } catch (Exception ex) {
                return View(model);
            }
        }

        [HttpGet]
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult Settings() {
            try {
                var user = _userService.GetUserById(UserId);
                return View(new SettingsViewModel(user));
            } catch (Exception ex) {
                return View();
            }
        }

        [HttpPost]
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult SaveSettings(SettingsViewModel model) {
            try {
                _settingsService.Insert(model.UnitSettings);
                
                var user = _userService.GetUserById(UserId);
                if (!user.Unit.SettingsId.HasValue) {
                    user.Unit.SettingsId = model.UnitSettings.Id;
                }
                if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword && _userService.ValidateLogin(model.Username, model.Password)) {
                    _userService.ChangePassword(user, model.Password);
                } else {
                    _userService.Insert(user);
                }
                return RedirectToAction("Dashboard", "Home");
            } catch (Exception ex) {
                return View();
            }
        }

        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult Signout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private void SaveUserState(User user, bool rememberMe) {
            var currentDateTime = DateTime.UtcNow.ToLocalTime();
            var userData = new UserDataModel { Id = user.Id, Role = user.Role };
            var authTicket = new FormsAuthenticationTicket(
                                    version: 1,
                                    name: user.Username,
                                    issueDate: currentDateTime,
                                    expiration: currentDateTime.AddMinutes(30),
                                    isPersistent: rememberMe,
                                    userData: JsonManager.Serialize(userData),
                                    cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket: authTicket);
            var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {
                HttpOnly = true
            };

            if (authTicket.IsPersistent)
                httpCookie.Expires = authTicket.Expiration;

            httpCookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
                httpCookie.Domain = FormsAuthentication.CookieDomain;

            HttpContext.Response.Cookies.Add(httpCookie);
        }
    }
}