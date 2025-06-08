using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;
using FootballServerGood.Model;

namespace FootballServerGood.DataAccess
{
    /// <summary>
    /// Handles database access related to player statistics, categorized by position-specific tables.
    /// </summary>
    public class StatsDB : BaseDB
    {
        /// <summary>
        /// Creates a new empty Stat entity instance.
        /// </summary>
        protected override BaseEntity newEntity()
        {
            return new Stat();
        }

        /// <summary>
        /// Maps one stat field from the reader to a Stat entity.
        /// Ignores "id" and "player_id" columns, returning the first actual stat field found.
        /// </summary>
        /// <param name="entity">The base entity to populate.</param>
        /// <returns>A Stat object with Name and Value from one stat column.</returns>
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Stat stat = (Stat)entity;

            // Skip id and player_id, take the first stat found
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                if (name != "id" && name != "player_id")
                {
                    stat.Name = name;
                    stat.Value = Convert.ToInt32(reader[i]);
                    break;
                }
            }

            return stat;
        }

        /// <summary>
        /// Not implemented. Inserting stat records is not supported directly.
        /// </summary>
        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented. Updating stat records is not supported directly.
        /// </summary>
        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented. Deleting stat records is not supported directly.
        /// </summary>
        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves all stats for a given player from a position-specific stats table (e.g., "gkStats", "defStats").
        /// </summary>
        /// <param name="position">The position string (e.g., "gk", "def", "mid", "att") used to determine the table name.</param>
        /// <param name="playerId">The ID of the player whose stats should be retrieved.</param>
        /// <returns>A list of Stat objects, each representing one stat field and value.</returns>
        public async Task<List<Stat>> SelectStatsByPosition(string position, int playerId)
        {
            string table = position.ToLower() + "Stats";
            string query = $"SELECT * FROM [{table}] WHERE player_id = {playerId}";

            List<Stat> stats = new List<Stat>();

            try
            {
                command.Connection = connection;
                command.CommandText = query;
                connection.Open();
                reader = (OleDbDataReader)await command.ExecuteReaderAsync();

                // Only one row per player expected
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string col = reader.GetName(i);
                        if (col == "id" || col == "player_id") continue;

                        stats.Add(new Stat
                        {
                            Name = col,
                            Value = Convert.ToInt32(reader[i])
                        });
                    }
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection.State == System.Data.ConnectionState.Open) connection.Close();
            }

            return stats;
        }
    }
}
