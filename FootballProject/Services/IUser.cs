using FootballProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballProject.Services
{
    /// <summary>
    /// Defines methods for managing users, teams, players, stats, and budgets in the football project.
    /// </summary>
    public interface IUser
    {
        // === USER METHODS ===

        /// <summary>
        /// Retrieves a list of all users in the system.
        /// </summary>
        Task<List<User>> GetAllUsers();

        /// <summary>
        /// Gets the currently logged-in user.
        /// </summary>
        User GetCurrentUser();

        /// <summary>
        /// Sets the currently logged-in user.
        /// </summary>
        /// <param name="user">The user to set as current.</param>
        void SetCurrentUser(User user);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        Task<User> GetUser(string username);

        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        Task<User> GetUserByID(int id);

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        Task<bool> AddUser(User user);

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        Task<bool> UpdateUser(User user);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        Task<bool> DeleteUser(User user);

        /// <summary>
        /// Initializes default users (e.g., for seeding/demo purposes).
        /// </summary>
        Task initUsers();

        // === TEAM METHODS ===

        /// <summary>
        /// Adds a new team to the system.
        /// </summary>
        Task<Team?> AddTeam(Team team);

        /// <summary>
        /// Retrieves all teams.
        /// </summary>
        Task<List<Team>> GetAllTeams();

        // === PLAYER METHODS ===

        /// <summary>
        /// Retrieves all players across all teams.
        /// </summary>
        Task<List<Player>> SelectAllPlayers();

        /// <summary>
        /// Retrieves all players belonging to a specific team.
        /// </summary>
        Task<List<Player>> SelectPlayersByTeam(int teamId);

        /// <summary>
        /// Retrieves players from a team that match a given first name.
        /// </summary>
        Task<List<Player>> SelectTeamPlayersByFirstName(int teamId, string name);

        /// <summary>
        /// Retrieves players filtered by a specified field and value.
        /// </summary>
        Task<List<Player>> SelectByFilter(string field, string value);

        /// <summary>
        /// Retrieves players sorted by a specified field and order.
        /// </summary>
        Task<List<Player>> SelectAndSort(string field, string order);

        // === STAT & BUDGET METHODS ===

        /// <summary>
        /// Retrieves all statistics for a specific player and position.
        /// </summary>
        Task<List<Stat>> GetAllStats(string position, int playerId);

        /// <summary>
        /// Retrieves all budget records associated with a specific team.
        /// </summary>
        Task<List<Budget>> GetBudgetsByTeamId(int teamId);

        /// <summary>
        /// Creates a new budget entry.
        /// </summary>
        Task<bool> CreateBudget(Budget budget);
    }
}
