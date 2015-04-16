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
    public class UserService : IUserService{

        private readonly IRepository<User> _userRepository;

        public UserService(){
            _userRepository = new Repository<User>();    
        }

        public User GetUserById(int id){
            return _userRepository.GetById(id);
        }

        public IList<User> GetAllUsers(){
            return _userRepository.Table.ToList();
        }

        public User GetByUsername(string username) {
            return _userRepository.Table.SingleOrDefault(x => x.Username == username.ToLower());
        }

        public void Insert(User user){
            try{
                if (user.Id == 0) {
                    var salt = "";
                    user.Username = user.Username.ToLower();
                    user.PasswordHash = SecurityHelper.HashPassword(user.PasswordHash, ref salt);
                    user.PasswordSalt = salt;
                    _userRepository.Insert(user);
                } else {
                    _userRepository.Update(user);
                }

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var user = _userRepository.GetById(id);
                _userRepository.Delete(user);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public bool ValidateLogin(string username, string password) {
            var user = _userRepository.Table.SingleOrDefault(u => u.Username == username.ToLower());
            if (user == null) return false;
            var salt = user.PasswordSalt;
            var pwHash = SecurityHelper.HashPassword(password: password, salt: ref salt);
            return user.PasswordHash == pwHash;
        }

        public bool ValidateUsername(string username) {
            return !_userRepository.Table.Any(x => x.Username == username.ToLower());
        }
    }
}
