using FootballProject.Model;
using FootballProject.ViewModel.DB;
using System.Collections.Generic;

namespace FootballProject.Services
{
    public class UserService
    {
        private List<User> users;
        private User currentUser;
        private readonly UserDB userDB = new UserDB();

        public UserService()
        {
            users = userDB.SelectAllUsers();
        }

        public List<User> GetUsersList()
        {
            return new List<User>(users);
        }

        public User GetUser(string username)
        {
            return userDB.SelectByUsername(username);
        }

        public User GetUserByID(int id)
        {
            return userDB.SelectById(id);
        }

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
            }
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public User GetCurrentUser()
        {
            return CurrentUser;
        }

        public bool AddUser(User user)
        {
            userDB.Insert(user);
            userDB.SaveChanges();
            users = userDB.SelectAllUsers(); // Refresh the local list
            return true;
        }

        public bool UpdateUser(User user)
        {
            userDB.Update(user);
            userDB.SaveChanges();
            users = userDB.SelectAllUsers(); // Refresh the local list
            return true;
        }

        public bool DeleteUser(User user)
        {
            userDB.Delete(user);
            userDB.SaveChanges();
            users = userDB.SelectAllUsers(); // Refresh the local list
            return true;
        }

        public List<User> GetAllUsers()
        {
            users = userDB.SelectAllUsers();
            return users;
        }
    }
}