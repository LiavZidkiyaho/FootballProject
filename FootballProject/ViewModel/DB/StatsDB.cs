using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;
using FootballProject.Model;

namespace FootballProject.ViewModel.DB
{
    public class StatsDB : BaseDB
    {
        protected override BaseEntity newEntity()
        {
            return new Stat();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Stat stat = (Stat)entity;

            // Skip id and player_id and just get the first stat column
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                if (name != "id" && name != "player_id")
                {
                    stat.Name = name;
                    stat.Value = Convert.ToInt32(reader[i]);
                    break; // Only take the first stat
                }
            }

            return stat;
        }


        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

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
