using FootballServerGood.Model;
using FootballServerGood.DataAccess;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace FootballServerGood.DataAccess
{
    /// <summary>
    /// Provides data access logic for the Users table, using OleDb and base CRUD methods.
    /// </summary>
    public class UserDB : BaseDB
    {
        /// <summary>
        /// Factory method to create a new User entity.
        /// </summary>
        protected override BaseEntity newEntity() => new User();

        /// <summary>
        /// Constructs the SQL INSERT query for a User entity.
        /// </summary>
        /// <param name="entity">The User entity to insert.</param>
        /// <returns>A SQL INSERT string.</returns>
        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $@"
INSERT INTO users (FullName, Username, UserPassword, Email, team, admin, Role)
VALUES ('{user.Name.Replace("'", "''")}', 
        '{user.Username.Replace("'", "''")}', 
        '{user.Password.Replace("'", "''")}', 
        '{user.Email.Replace("'", "''")}', 
        {user.Team.Id}, 
        '{user.IsAdmin.Replace("'", "''")}', 
        '{user.Role.Replace("'", "''")}')";
        }

        /// <summary>
        /// Constructs the SQL UPDATE query for a User entity.
        /// </summary>
        /// <param name="entity">The User entity to update.</param>
        /// <returns>A SQL UPDATE string.</returns>
        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $@"
UPDATE users 
SET FullName = '{user.Name.Replace("'", "''")}', 
    Username = '{user.Username.Replace("'", "''")}', 
    UserPassword = '{user.Password.Replace("'", "''")}', 
    Email = '{user.Email.Replace("'", "''")}', 
    team = {user.Team.Id}, 
    admin = '{user.IsAdmin.Replace("'", "''")}', 
    Role = '{user.Role.Replace("'", "''")}'
WHERE id = {user.Id}";
        }

        /// <summary>
        /// Constructs the SQL DELETE query for a User entity.
        /// </summary>
        /// <param name="entity">The User entity to delete.</param>
        /// <returns>A SQL DELETE string.</returns>
        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var user = (User)entity;
            return $"DELETE FROM users WHERE id = {user.Id}";
        }

        /// <summary>
        /// Creates a User model instance from the current reader row.
        /// </summary>
        /// <param name="entity">BaseEntity to populate as a User.</param>
        /// <returns>Populated User object.</returns>
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var user = (User)entity;
            user.Id = reader.GetInt32(0);
            user.Name = reader.GetString(1);
            user.Username = reader.GetString(2);
            user.Password = reader.GetString(3);
            user.Email = reader.GetString(4);

            user.Team = new Team
            {
                Id = reader.GetInt32(5),
                team1 = reader.GetString(8)
            };

            user.IsAdmin = reader.GetString(6);
            user.Role = reader.GetString(7);
            return user;
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>List of all User entities.</returns>
        public async Task<List<User>> SelectAllUsers()
        {
            string query = @"
            SELECT u.*, t.Team 
            FROM users u
            INNER JOIN Team t ON u.team = t.id";

            List<BaseEntity> list = await Select(query);
            return list.Cast<User>().ToList();
        }

        /// <summary>
        /// Retrieves a single user by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User if found; otherwise, null.</returns>
        public async Task<User> SelectById(int id)
        {
            string query = $@"
            SELECT u.*, t.Team 
            FROM users u
            INNER JOIN Team t ON u.team = t.id
            WHERE u.id = {id}";

            List<BaseEntity> list = await Select(query);
            return list.Count == 1 ? list[0] as User : null;
        }

        /// <summary>
        /// Retrieves a single user by their username.
        /// </summary>
        /// <param name="username">Username string.</param>
        /// <returns>User if found; otherwise, null.</returns>
        public async Task<User> SelectByUsername(string username)
        {
            string safeUsername = username.Replace("'", "''");
            string query = $@"
            SELECT u.*, t.Team 
            FROM users u 
            LEFT JOIN Team t ON u.team = t.id 
            WHERE u.Username = '{safeUsername}'";

            List<BaseEntity> list = await Select(query);
            return list.Count == 1 ? list[0] as User : null;
        }

        /// <summary>
        /// Adds a User entity to the insert queue.
        /// </summary>
        /// <param name="entity">User to insert.</param>
        public override void Insert(BaseEntity entity)
        {
            if (entity is User)
                inserted.Add(new ChangeEntity(this.CreateInsertOleDb, entity));
        }

        /// <summary>
        /// Adds a User entity to the update queue.
        /// </summary>
        /// <param name="entity">User to update.</param>
        public override void Update(BaseEntity entity)
        {
            if (entity is User)
                updated.Add(new ChangeEntity(this.CreateUpdateOleDb, entity));
        }

        /// <summary>
        /// Adds a User entity to the delete queue.
        /// </summary>
        /// <param name="entity">User to delete.</param>
        public override void Delete(BaseEntity entity)
        {
            if (entity is User)
                deleted.Add(new ChangeEntity(this.CreateDeleteOleDb, entity));
        }
    }
}
