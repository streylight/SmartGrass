using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the user service
    /// </summary>
    public interface IUserService{
        User GetUserById(int id);
        IList<User> GetAllUsers();
        void Insert(User user);
        void Delete(int id);
        bool ValidateLogin(string username, string password);
        bool ValidateUsername(string username);
        User GetByUsername(string username);
        void ChangePassword(User account, string newPassword);
    }
}
