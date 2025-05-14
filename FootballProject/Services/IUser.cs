using FootballProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballProject.Services
{
    public interface IUser
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(string username);
        Task<User> GetUserByID(int id);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);

        Task<List<Stat>> GetAllStats(string position, int playerId);
    }
}
