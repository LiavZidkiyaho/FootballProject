using FootballServerGood.Model;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace FootballServerGood.DataAccess
{
    /// <summary>
    /// Handles database operations for Player entities, including filtering, sorting, and team-based queries.
    /// </summary>
    public class PlayerDB : BaseDB
    {
        /// <summary>
        /// Returns a new empty Player instance for casting base entities.
        /// </summary>
        protected override BaseEntity newEntity()
        {
            return new Player();
        }

        /// <summary>
        /// Throws NotImplementedException – inserting players is not supported via this DB.
        /// </summary>
        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException – updating players is not supported via this DB.
        /// </summary>
        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException – deleting players is not supported via this DB.
        /// </summary>
        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the current row in the reader to a Player object.
        /// </summary>
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var player = (Player)entity;
            player.Id = reader.GetInt32(0);
            player.FullName = reader.GetString(1);
            player.Nationality = reader.GetString(2);
            player.DateOfBirth = reader.GetDateTime(3);

            player.Team = new Team();
            player.Team.Id = reader.GetInt32(4);
            player.Team.team1 = reader.GetString(11); // team name column from JOINed Team table

            player.UserValue = reader.GetInt32(5);
            player.Wage = reader.GetInt32(6);
            player.Height = reader.GetInt32(7);
            player.Weight = reader.GetInt32(8);
            player.Foot = reader.GetString(9);
            player.Position = reader.GetString(10);
            return player;
        }

        /// <summary>
        /// Gets all players with their teams.
        /// </summary>
        public async Task<List<Player>> SelectAllPlayers()
        {
            string query = @"
    SELECT p.*, t.Team 
    FROM players p
    INNER JOIN Team t ON p.team = t.id";

            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Player>().ToList();
        }

        /// <summary>
        /// Gets a single player by ID, including team info.
        /// </summary>
        public async Task<Player> SelectById(int id)
        {
            string query = $@"
    SELECT p.*, t.Team 
    FROM players p
    INNER JOIN Team t ON p.team = t.id
    WHERE p.id = {id}";

            List<BaseEntity> list = await base.Select(query);
            if (list != null && list.Count == 1)
                return list[0] as Player;
            return null;
        }

        /// <summary>
        /// Filters players by any field or by a specific one.
        /// </summary>
        /// <param name="field">Field to filter (or "Any" for all fields)</param>
        /// <param name="value">Value to match</param>
        public async Task<List<Player>> SelectByFilter(string field, string value)
        {
            string query;
            if (field == "Any")
            {
                query = $@"
        SELECT p.*, t.Team 
        FROM players p
        INNER JOIN Team t ON p.team = t.id
        WHERE p.FullName LIKE '%{value}%' OR p.Nationality LIKE '%{value}%' OR p.DateOfBirth LIKE '%{value}%' 
            OR t.Team LIKE '%{value}%' OR p.UserValue LIKE '%{value}%' OR p.Wage LIKE '%{value}%' 
            OR p.Height LIKE '%{value}%' OR p.Weight LIKE '%{value}%' OR p.Foot LIKE '%{value}%' 
            OR p.Position LIKE '%{value}%'";
            }
            else
            {
                query = $@"
        SELECT p.*, t.Team 
        FROM players p
        INNER JOIN Team t ON p.team = t.id
        WHERE p.{field} LIKE '{value}'";
            }

            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Player>().ToList();
        }

        /// <summary>
        /// Returns players sorted by a specific field and order.
        /// </summary>
        /// <param name="field">Sort field</param>
        /// <param name="order">"ASC" or "DESC"</param>
        public async Task<List<Player>> SelectAndSort(string field, string order)
        {
            string query = $@"
    SELECT p.*, t.Team 
    FROM players p
    INNER JOIN Team t ON p.team = t.id
    ORDER BY p.{field} {order}";

            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Player>().ToList();
        }

        /// <summary>
        /// Gets all players belonging to a specific team.
        /// </summary>
        public async Task<List<Player>> SelectPlayersByTeam(int teamId)
        {
            string query = $@"
        SELECT p.*, t.Team 
        FROM players p
        INNER JOIN Team t ON p.team = t.id
        WHERE p.team = {teamId}";

            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Player>().ToList();
        }

        /// <summary>
        /// Gets players by partial name match within a specific team.
        /// </summary>
        public async Task<List<Player>> SelectTeamPlayersByFirstName(int teamId, string firstName)
        {
            string query = $@"
        SELECT p.*, t.Team 
        FROM players p
        INNER JOIN Team t ON p.team = t.id
        WHERE p.team = {teamId} AND p.full_name LIKE '%{firstName}%'";

            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Player>().ToList();
        }
    }
}
