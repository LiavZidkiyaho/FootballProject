using FootballServerGood.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballServerGood.DataAccess
{
    /// <summary>
    /// Provides data access functionality for the Team table in the database.
    /// Inherits base functionality from BaseDB for managing insert, update, delete, and select operations.
    /// </summary>
    public class TeamDB : BaseDB
    {
        /// <summary>
        /// Returns a new Team entity instance for use in factory pattern methods.
        /// </summary>
        protected override BaseEntity newEntity()
        {
            return new Team();
        }

        /// <summary>
        /// Populates a Team entity based on the current row in the OleDbDataReader.
        /// </summary>
        /// <param name="entity">The base entity to populate.</param>
        /// <returns>The populated Team object.</returns>
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var team = (Team)entity;

            team.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            team.team1 = reader.GetString(reader.GetOrdinal("Team"));

            return team;
        }

        /// <summary>
        /// Generates the SQL insert statement for a given Team entity.
        /// </summary>
        /// <param name="entity">The Team entity to insert.</param>
        /// <returns>A valid SQL insert statement string.</returns>
        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            var team = (Team)entity;
            return $@"
INSERT INTO Team (Team)
VALUES ('{team.team1.Replace("'", "''")}')";
        }

        /// <summary>
        /// Generates the SQL update statement for a given Team entity.
        /// </summary>
        /// <param name="entity">The Team entity to update.</param>
        /// <returns>A valid SQL update statement string.</returns>
        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var team = (Team)entity;
            return $@"
UPDATE Team 
SET Team = '{team.team1.Replace("'", "''")}'
WHERE Id = {team.Id}";
        }

        /// <summary>
        /// Generates the SQL delete statement for a given Team entity.
        /// </summary>
        /// <param name="entity">The Team entity to delete.</param>
        /// <returns>A valid SQL delete statement string.</returns>
        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var team = (Team)entity;
            return $"DELETE FROM Team WHERE Id = {team.Id}";
        }

        /// <summary>
        /// Retrieves all teams from the Team table.
        /// </summary>
        /// <returns>A list of all Team objects.</returns>
        public async Task<List<Team>> SelectAllTeams()
        {
            string query = "SELECT * FROM Team";
            List<BaseEntity> list = await Select(query);
            return list.Cast<Team>().ToList();
        }

        /// <summary>
        /// Retrieves a single team by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the team.</param>
        /// <returns>The Team object if found, or null if not found.</returns>
        public async Task<Team> SelectById(int id)
        {
            string query = $"SELECT * FROM Team WHERE Id = {id}";
            List<BaseEntity> list = await Select(query);
            return list.Cast<Team>().FirstOrDefault();
        }
    }
}
