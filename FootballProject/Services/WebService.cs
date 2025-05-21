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
    public class WebService : IUser
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;
        private const string baseUrl = "http://localhost:5026";

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

        public async Task<List<User>> GetAllUsers()
        {
            var response = await client.GetAsync($"{baseUrl}/api/users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, options);
        }

        public async Task<User> GetUser(string username)
        {
            var response = await client.GetAsync($"{baseUrl}/api/users/by-username/{username}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(json, options);
        }

        public async Task<User> GetUserByID(int id)
        {
            var response = await client.GetAsync($"{baseUrl}/api/users/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(json, options);
        }

        public async Task<bool> AddUser(User user)
        {
            var content = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/users", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var content = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/api/users/{user.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(User user)
        {
            var response = await client.DeleteAsync($"{baseUrl}/api/users/{user.Id}");
            return response.IsSuccessStatusCode;
        }

        public async Task initUsers()
        {
            await Task.CompletedTask;
        }

        public async Task InitStats(int id, string position = null)
        {
            await Task.CompletedTask;
        }

        public async Task<List<Stat>> GetAllStats(string position, int id)
        {
            var response = await client.GetAsync($"{baseUrl}/api/stats/{position}/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Stat>>(json, options);
        }

        // ---------------------- BUDGET METHODS ----------------------

        public async Task<Budget> GetBudgetByTeamId(int teamId)
        {
            var response = await client.GetAsync($"{baseUrl}/api/budgets/team/{teamId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Budget>(json, options);
        }

        public async Task<bool> UpdateBudget(Budget budget)
        {
            var content = new StringContent(JsonSerializer.Serialize(budget, options), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/api/budgets/{budget.TeamId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateYearlyBudget(Budget budget)
        {
            var content = new StringContent(JsonSerializer.Serialize(budget, options), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/api/yearlybudgets/{budget.Total}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSeasonBudget(Budget budget)
        {
            var content = new StringContent(JsonSerializer.Serialize(budget, options), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/api/seasonbudgets/{budget.ProfitLose}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task SaveChanges()
        {
            await Task.CompletedTask; // No-op in web context
        }

        // ---------------------- PLAYER METHODS ----------------------

        public async Task<List<Player>> SelectAllPlayers()
        {
            var response = await client.GetAsync($"{baseUrl}/api/players");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        public async Task<List<Player>> SelectPlayersByTeam(int teamId)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/team/{teamId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        public async Task<List<Player>> SelectTeamPlayersByFirstName(int teamId, string name)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/team/{teamId}/filter?name={name}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        public async Task<List<Player>> SelectByFilter(string field, string value)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/filter?field={field}&value={value}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }

        public async Task<List<Player>> SelectAndSort(string field, string order)
        {
            var response = await client.GetAsync($"{baseUrl}/api/players/sort?field={field}&order={order}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Player>>(json, options);
        }
    }
}
