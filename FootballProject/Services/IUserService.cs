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
        private readonly StatsDB statsDB = new StatsDB();
        private List<Stat> stats;

        public UserService()
        {

        }

        public async Task initUsers(string position = null)
        {
            users = (await userDB.SelectAllUsers()).Cast<User>().ToList();
        }

        public async Task initStats(int id, string position = null)
        {
            
        }

        public List<User> GetUsersList()
        {
            return new List<User>(users);
        }

        public async Task<User> GetUser(string username)
        {
            return await userDB.SelectByUsername(username);
        }

        public async Task<User> GetUserByID(int id)
        {
            return await userDB.SelectById(id);
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

        public async Task<bool> AddUser(User user)
        {
            userDB.Insert(user);
            userDB.SaveChanges();
            users = await userDB.SelectAllUsers(); // Refresh the local list
            return true;
        }

        public async Task<bool> UpdateUser(User user)
        {
            userDB.Update(user);
            userDB.SaveChanges();
            users = await userDB.SelectAllUsers(); // Refresh the local list
            return true;
        }

        public async Task<bool> DeleteUser(User user)
        {
            userDB.Delete(user);
            userDB.SaveChanges();
            users = await userDB.SelectAllUsers(); // Refresh the local list
            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            users = await userDB.SelectAllUsers();
            return users;
        }

        public async Task<List<Stat>> GetAllStats(string position, int id)
        {
            stats = await statsDB.SelectStatsByPosition(position, id);
            return stats;
        }
    }
}