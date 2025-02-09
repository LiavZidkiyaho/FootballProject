using FootballProject.Model;
using System.Collections.Generic;
using System.Data.OleDb;

namespace FootballProject.ViewModel.DB
{
    public class UserDB : BaseDB
    {
        protected override BaseEntity newEntity()
        {
            return new User();
        }

        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $"INSERT INTO users (Username, UserPassword) VALUES ('{user.Username}', '{user.Password}')";
        }

        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $"UPDATE users SET Username = '{user.Username}', UserPassword = '{user.Password}' WHERE id = {user.Id}";
        }

        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $"DELETE FROM users WHERE id = {user.Id}";
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var user = (User)entity;
            user.Id = reader.GetInt32(0);
            user.Username = reader.GetString(1);
            user.Password = reader.GetString(2);
            return user;
        }

        public List<User> SelectAllUsers()
        {
            string query = "SELECT * FROM users";
            var result = Select(query);
            return result.ConvertAll(x => (User)x);
        }

        public User SelectById(int id)
        {
            string query = $"SELECT * FROM users WHERE id = {id}";
            var result = Select(query);
            return result.Count > 0 ? (User)result[0] : null;
        }

        public User SelectByUsername(string username)
        {
            string query = $"SELECT * FROM users WHERE Username = '{username}'";
            var result = Select(query);
            return result.Count > 0 ? (User)result[0] : null;

        }
    }
}