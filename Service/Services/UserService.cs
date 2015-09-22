using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;
using Core.Helpers.Security;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the user class
    /// </summary>
    public class UserService : IUserService {

        #region vars

        private readonly IRepository<User> _userRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the UserService class
        /// </summary>
        public UserService() {
            _userRepository = new Repository<User>();    
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a User with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById( int id ) {
            return _userRepository.GetById( id );
        }

        /// <summary>
        /// Returns all Users
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAllUsers() {
            return _userRepository.Table.ToList();
        }

        /// <summary>
        /// Returns a user with matching username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetByUsername( string username ) {
            return _userRepository.Table.SingleOrDefault( x => x.Username == username.ToLower() );
        }

        /// <summary>
        /// Hash new user password and insert the user into the db
        /// </summary>
        /// <param name="user"></param>
        public void Insert( User user ){
            if ( user.Id == 0 ) {
                var salt = "";
                user.Username = user.Username.ToLower();
                user.PasswordHash = SecurityHelper.HashPassword( user.PasswordHash, ref salt );
                user.PasswordSalt = salt;
                _userRepository.Insert( user );
            } else {
                _userRepository.Update( user );
            }
        }

        /// <summary>
        /// Deletes a User from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete( int id ){
            var user = _userRepository.GetById( id );
            if ( user == null ) {
                throw new Exception( string.Format( "No irrigation valve found with id: {0}", id ) );
            }
            _userRepository.Delete( user );
        }

        /// <summary>
        /// Validates a login attempt
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateLogin( string username, string password ) {
            var user = _userRepository.Table.SingleOrDefault( u => u.Username == username.ToLower() );
            if ( user == null ) {
                return false;
            }
            var salt = user.PasswordSalt;
            var pwHash = SecurityHelper.HashPassword( password: password, salt: ref salt );
            return user.PasswordHash == pwHash;
        }

        /// <summary>
        /// Updates the user's password with the new password
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPassword"></param>
        public void ChangePassword( User account, string newPassword ) {
            var salt = "";
            account.PasswordHash = SecurityHelper.HashPassword( newPassword, ref salt );
            account.PasswordSalt = salt;
            _userRepository.Update( account );
        }

        /// <summary>
        /// Check if a username for registration is unique
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ValidateUsername( string username ) {
            return !_userRepository.Table.Any( x => x.Username == username.ToLower() );
        }

        #endregion
    } // class
} // namespace
