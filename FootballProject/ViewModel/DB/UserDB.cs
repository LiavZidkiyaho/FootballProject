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
            return $"INSERT INTO users (FullName, Username, UserPassword, Email, team, admin) VALUES ('{user.Name}', '{user.Username}', '{user.Password}', '{user.Email}','{user.Team}','{user.IsAdmin}')";
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
            user.Name= reader.GetString(1);
            user.Username = reader.GetString(2);
            user.Password = reader.GetString(3);
            user.Email = reader.GetString(4);
            user.Team = reader.GetString(5);
            user.IsAdmin = reader.GetString(6);
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

        public override void Insert(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                inserted.Add(new ChangeEntity(this.CreateInsertOleDb, entity));
            }
        }

        public override void Update(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                updated.Add(new ChangeEntity(this.CreateUpdateOleDb, entity));
            }
        }

        public override void Delete(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                deleted.Add(new ChangeEntity(this.CreateDeleteOleDb, entity));
            }
        }
    }
}