using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballProject.Services
{
    public class UserService : IUser
    {
        private List<User> users = new List<User>
        {
            new User { Username = "test", Password = "password" }
        };

        public bool AddUser(User user)
        {
            users.Add(user);
            return true;
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
