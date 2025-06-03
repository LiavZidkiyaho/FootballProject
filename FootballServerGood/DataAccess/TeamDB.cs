using FootballServerGood.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballServerGood.DataAccess
{
    public class TeamDB : BaseDB
    {
        protected override BaseEntity newEntity()
        {
            return new Team();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var team = (Team)entity;

            team.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            team.team1 = reader.GetString(reader.GetOrdinal("Team"));

            return team;
        }

        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            var team = (Team)entity;
            return $@"
INSERT INTO Team (TeamName)
VALUES ('{team.team1.Replace("'", "''")}')";
        }

        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var team = (Team)entity;
            return $@"
UPDATE Team 
SET TeamName = '{team.team1.Replace("'", "''")}'
WHERE Id = {team.Id}";
        }

        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var team = (Team)entity;
            return $"DELETE FROM Team WHERE Id = {team.Id}";
        }

        public async Task<List<Team>> SelectAllTeams()
        {
            string query = "SELECT * FROM Team";
            List<BaseEntity> list = await Select(query);
            return list.Cast<Team>().ToList();
        }

        public async Task<Team> SelectById(int id)
        {
            string query = $"SELECT * FROM Team WHERE Id = {id}";
            List<BaseEntity> list = await Select(query);
            return list.Cast<Team>().FirstOrDefault();
        }
    }
}
