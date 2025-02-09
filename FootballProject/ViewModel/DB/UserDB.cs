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
            return $"INSERT INTO Users (Username, Password) VALUES ('{user.Username}', '{user.Password}')";
        }

        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $"UPDATE Users SET Username = '{user.Username}', Password = '{user.Password}' WHERE Id = {user.Id}";
        }

        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $"DELETE FROM Users WHERE Id = {user.Id}";
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var user = (User)entity;
            user.Id = reader.GetInt32(0);
            user.Username = reader.GetString(1);
            user.Password = reader.GetString(2);
            // Add other properties as needed
            return user;
        }

        public List<User> SelectAllUsers()
        {
            string query = "SELECT * FROM Users";
            var result = Select(query);
            return result.ConvertAll(x => (User)x);
        }

        public User SelectById(int id)
        {
            string query = $"SELECT * FROM Users WHERE Id = {id}";
            var result = Select(query);
            return result.Count > 0 ? (User)result[0] : null;
        }

        public User SelectByUsername(string username)
        {
            string query = $"SELECT * FROM Users WHERE Username = '{username}'";
            var result = Select(query);
            return result.Count > 0 ? (User)result[0] : null;
        }
    }
}