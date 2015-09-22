﻿using System;
using System.Linq;
using System.Web.Mvc;
using Core;
using Service.Interfaces;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers {
    /// <summary>
    /// The admin controller
    /// </summary>
    public class AdminController : BaseController {

        #region vars

        private readonly IUserService _userService;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the AdminController setup for dependency injection
        /// </summary>
        /// <param name="userService"></param>
        public AdminController( IUserService userService ) {
            _userService = userService;
        }

        #endregion

        #region methods

        /// <summary>
        /// Load the admin dashboard page
        /// </summary>
        /// <returns></returns>
        [AppAuthorize(Role.Admin)]
        public ActionResult AdminDashboard() {
            try {
                var model = _userService.GetAllUsers().Where( x => x.Role == Role.User ).ToList();
                return View( model );
            } catch ( Exception ex ) {
                // TODO: add error reporting
                return View();
            }
        }

        /// <summary>
        /// Deletes a user account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(Role.Admin)]
        public ActionResult DeleteUserAccount( int id ) {
            try {
                _userService.Delete( id );
                return Json( new { msg = "User account successfully deleted" } );
            } catch ( Exception ex ) {
                return Json( new { msg = ex.Message, error = true } );
            } 
        }

        /// <summary>
        /// Updates a user's account information
        /// POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(Role.Admin)]
        public ActionResult UpdateUserAccount( UserAccountViewModel model ) {
            try {
                var user = _userService.GetUserById( model.User.Id );
                
                if ( _userService.GetAllUsers().Any( x => x.Username == model.User.Username.ToLower() && x.Id != user.Id ) ) {
                    return Json( new { msg = "That username already exists on another account", error = true } );
                }
                user.Username = model.User.Username.ToLower();
                user.Role = model.User.Role;
                user.Unit.ProductKey = model.Unit.ProductKey;
                if ( !string.IsNullOrEmpty( model.User.PasswordHash ) ) {
                    _userService.ChangePassword( user, model.User.PasswordHash );
                } else {
                    _userService.Insert( user );
                }

                return Json( new { username = user.Username, pkey = user.Unit.ProductKey, msg = "User account successfully updated" } );
            } catch ( Exception ex ) {
                return Json( new { msg = ex.Message, error = true } );
            }
        }

        /// <summary>
        /// Load the user account details page
        /// GET
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AppAuthorize(Role.Admin)]
        [HttpGet]
        public ActionResult LoadUserAccountDetails( int userId  ) {
            try {
                var user = _userService.GetUserById( userId );
                var model = new UserAccountViewModel {
                    User = user,
                    Unit = user.Unit
                };
                model.User.Unit = null;
                return View( "_UserAccountForm", model );
            } catch ( Exception ex ) {
                return View();
            }
        }

        #endregion
    } // class
} // namespace