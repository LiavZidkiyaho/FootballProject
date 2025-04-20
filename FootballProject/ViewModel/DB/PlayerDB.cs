using FootballProject.Model;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace FootballProject.ViewModel.DB
{
    public class PlayerDB : BaseDB
    {
        protected override BaseEntity newEntity()
        {
            return new Player();
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

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var player = (Player)entity;
            player.Id = reader.GetInt32(0);
            player.FullName = reader.GetString(1);
            player.Nationality = reader.GetString(2);
            player.DateOfBirth = reader.GetDateTime(3);

            player.Team = new Team();
            player.Team.Id = reader.GetInt32(4);
            player.Team.team1 = reader.GetString(11);

            player.UserValue = reader.GetInt32(5);
            player.Wage = reader.GetInt32(6);
            player.Height = reader.GetInt32(7);
            player.Weight = reader.GetInt32(8);
            player.Foot = reader.GetString(9);
            player.Position = reader.GetString(10);
            return player;
        }

        public async Task<List<Player>> SelectAllPlayers()
        {
            string query = @"
    SELECT p.*, t.Team 
    FROM players p
    INNER JOIN Team t ON p.team = t.id";

            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Player>().ToList();
        }


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
            OR p.Team LIKE '%{value}%' OR p.UserValue LIKE '%{value}%' OR p.Wage LIKE '%{value}%' 
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

    }
}