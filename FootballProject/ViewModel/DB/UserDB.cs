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
            return $"UPDATE users SET FullName = '{user.Name}', Username = '{user.Username}', UserPassword = '{user.Password}', Email = '{user.Email}', team = '{user.Team}', admin = '{user.IsAdmin}' WHERE id = {user.Id}";
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

        public async Task<List<User>> SelectAllUsers()
        {
            string query = "SELECT * FROM users";
            List<BaseEntity> list = await base.Select(query);
            return new UserList(list);
        }

        public async Task<User> SelectById(int id)
        {
            string query = $"SELECT * FROM users WHERE id = {id}";
            List<BaseEntity> list = await base.Select(query);
            if(list != null && list.Count == 1)
                return list[0] as User;
            return null;
        }

        public async Task<User> SelectByUsername(string username)
        {
            string query = $"SELECT * FROM users WHERE Username = '{username}'";
            List<BaseEntity> list = await base.Select(query);
            if (list != null && list.Count == 1)
                return list[0] as User;
            return null;
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