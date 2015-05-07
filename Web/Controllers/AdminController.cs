using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Service.Interfaces;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers {
    public class AdminController : BaseController {

        private readonly IUserService _userService;

        public AdminController(IUserService userService) {
            _userService = userService;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminDashboard() {
            try {
                var model = _userService.GetAllUsers().Where(x => x.Role == Role.User).ToList();
                return View(model);
            } catch (Exception) {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeleteUserAccount(int id) {
            try {
                _userService.Delete(id);
                return Json(new {msg = "User account successfully deleted"});
            } catch (Exception ex) {
                return Json(new {msg = ex.Message, error = true});
            } 
        }

        public ActionResult UpdateUserAccount(UserAccountViewModel model) {
            try {
                var user = _userService.GetUserById(model.User.Id);
                
                if (_userService.GetAllUsers().Any(x => x.Username == model.User.Username.ToLower() && x.Id != user.Id)) {
                    return Json(new { msg = "That username already exists on another account", error = true });
                }
                user.Username = model.User.Username.ToLower();
                user.Role = model.User.Role;
                user.Unit.ProductKey = model.Unit.ProductKey;
                if (!string.IsNullOrEmpty(model.User.PasswordHash)) {
                    _userService.ChangePassword(user, model.User.PasswordHash);
                } else {
                    _userService.Insert(user);
                }

                return Json(new { username = user.Username, pkey = user.Unit.ProductKey, msg = "User account successfully updated" });
            } catch (Exception ex) {
                return Json(new { msg = ex.Message, error = true });
            }
        }

        public ActionResult LoadUserAccountDetails(int userId) {
            try {
                var user = _userService.GetUserById(userId);
                var model = new UserAccountViewModel {
                    User = user,
                    Unit = user.Unit
                };
                model.User.Unit = null;
                return View("_UserAccountForm", model);
            } catch (Exception ex) {
                return View();
            }
        }
    }
}