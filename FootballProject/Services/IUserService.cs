using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace FootballProject.Services
{
    public class UserService
    {
        private List<Model.User> users = new List<Model.User>();
        public Model.User CurrentUser;

        Model.User tempUser = new Model.User() { Id = 1, Username = "Bruh", Password = "Bruh12", Email = "Bruh@gmail.com" };
        Model.User tempOtherUser = new Model.User() { Id = 2, Username = "admin", Password = "admin", Email = "admin@gmail.com" };

        public UserService()
        {
            users = GetUsersList();
            users.Add(tempUser);
            users.Add(tempOtherUser);
        }

        public List<Model.User> GetUsersList()
        {
            return users;
        }

        public Model.User GetUser(string username)
        {
            foreach (Model.User usr in users)
            {
                if (usr.Username == username) return usr;
            }
            return null;
        }
        public Model.User GetUserByID(int id)
        {
            foreach (Model.User usr in users)
            {
                if (usr.Id == id) return usr;
            }
            return null;
        }
    }
}
