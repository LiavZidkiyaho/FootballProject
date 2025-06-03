using FootballProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballProject.Services
{
    public interface IUser
    {
        Task<List<User>> GetAllUsers();
        User GetCurrentUser();
        void SetCurrentUser(User user);
        Task<User> GetUser(string username);
        Task<User> GetUserByID(int id);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);

        Task<List<Team>> GetAllTeams();

        Task<List<Stat>> GetAllStats(string position, int playerId);
        Task<List<Budget>> GetBudgetsByTeamId(int teamId);

        Task<bool> CreateBudget(Budget budget);

        Task<List<Player>> SelectAllPlayers();
        Task<List<Player>> SelectPlayersByTeam(int teamId);
        Task<List<Player>> SelectTeamPlayersByFirstName(int teamId, string name);
        Task<List<Player>> SelectByFilter(string field, string value);
        Task<List<Player>> SelectAndSort(string field, string order);

        public Task initUsers();
    }
}
