using FootballProject.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FootballProject.Services
{
    /// <summary>
    /// A concrete implementation of the IUser service that communicates with a remote REST API
    /// to manage users, players, teams, statistics, and budgets.
    /// </summary>
    public class WebService : IUser
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;
        private const string baseUrl = "https://hjjm22x3-5026.euw.devtunnels.ms";

        /// <summary>
        /// Initializes a new instance of the WebService class with configured HTTP and JSON settings.
        /// </summary>
        public WebService()
        {
            client = new HttpClient();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
        }

        // ---------------------- USER METHODS ----------------------

        /// <inheritdoc/>
        public async Task<List<User>> GetAllUsers()
        {
            var response = await client.GetAsync($"{baseUrl}/api/users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, options);
        }

        private User currentUser;

        /// <inheritdoc/>
        public User GetCurrentUser() => currentUser;

        /// <inheritdoc/>
        public void SetCurrentUser(User user) => currentUser = user;

        /// <inheritdoc/>
        public async Task<User> GetUser(string username)
        {
            var response = await client.GetAsync($"{baseUrl}/api/users/by-username/{username}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(json, options);
        }

        /// <inheritdoc/>
        public async Task<User> GetUserByID(int id)
        {
            var response = await client.GetAsync($"{baseUrl}/api/users/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(json, options);
        }

        /// <inheritdoc/>
        public async Task<bool> AddUser(User user)
        {
            var content = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/users", content);
            return response.IsSuccessStatusCode;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateUser(User user)
        {
            var content = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/api/users/{user.Id}", content);
            return response.IsSuccessStatusCode;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteUser(User user)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/users/{user.Id}");
            return response.IsSuccessStatusCode;
        }

        /// <inheritdoc/>
        public async Task initUsers()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Placeholder method for future stat initialization.
        /// </summary>
        public async Task InitStats(int id, string position = null)
        {
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<List<Stat>> GetAllStats(string position, int id)
        {
            var response = await client.GetAsync($"{baseUrl}/api/stats/{position}/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Stat>>(json, options);
        }

        // ---------------------- BUDGET METHODS ----------------------

        /// <inheritdoc/>
        public async Task<List<Budget>> GetBudgetsByTeamId(int teamId)
        {
            var response = await client.GetAsync($"{baseUrl}/api/budgets/team/{teamId}");
            if (!response.IsSuccessStatusCode) return new List<Budget>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Budget>>(json, options);
        }

        /// <summary>
        /// Updates an existing budget entry.
        /// </summary>
        public async Task<bool> UpdateBudget(Budget budget)
        {
            var content = new StringContent(JsonSerializer.Serialize(budget, options), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/api/budgets/{budget.Id}", content);
            return response.IsSuccessStatusCode;
        }

        /// <inheritdoc/>
        public async Task<bool> CreateBudget(Budget budget)
        {
            var content = new StringContent(JsonSerializer.Serialize(budget, options), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/budgets", content);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Deletes a budget entry by ID.
        /// </summary>
        public async Task<bool> DeleteBudget(int budgetId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/budgets/{budgetId}");
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Placeholder method for commit-style API design.
        /// </summary>
        public async Task SaveChanges()
        {
            await Task.CompletedTask;
        }

        // ---------------------- PLAYER METHODS ----------------------

        /// <inheritdoc/>
        public async Task<List<Player>> SelectAllPlayers()
        {
            var response = await client.GetAsync($"{baseUrl}/api/players");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        /// <inheritdoc/>
        public async Task<List<Player>> SelectPlayersByTeam(int teamId)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/team/{teamId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        /// <inheritdoc/>
        public async Task<List<Player>> SelectTeamPlayersByFirstName(int teamId, string name)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/team/{teamId}/filter?name={name}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        /// <inheritdoc/>
        public async Task<List<Player>> SelectByFilter(string field, string value)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/filter?field={field}&value={value}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        /// <inheritdoc/>
        public async Task<List<Player>> SelectAndSort(string field, string order)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/sort?field={field}&order={order}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        // ---------------------- TEAM METHODS ----------------------

        /// <inheritdoc/>
        public async Task<List<Team>> GetAllTeams()
        {
            var response = await client.GetAsync($"{baseUrl}/api/team");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Team>>(json, options);
        }

        /// <inheritdoc/>
        public async Task<Team?> AddTeam(Team team)
        {
            var content = new StringContent(JsonSerializer.Serialize(team, options), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/team", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Team>(json, options);
        }
    }
}
