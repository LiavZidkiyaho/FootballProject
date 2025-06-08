using System;
using System.Collections.Generic;
using System.Data.OleDb;
using FootballServerGood.Model;

namespace FootballServerGood.DataAccess
{
    /// <summary>
    /// Base class for handling common database operations (CRUD) using OleDb for MS Access.
    /// </summary>
    public abstract class BaseDB
    {
        private string connectionString;
        protected OleDbConnection connection;
        protected OleDbCommand command;
        protected OleDbDataReader reader;

        protected List<ChangeEntity> inserted = new List<ChangeEntity>();
        protected List<ChangeEntity> deleted = new List<ChangeEntity>();
        protected List<ChangeEntity> updated = new List<ChangeEntity>();

        /// <summary>
        /// Creates an empty instance of the entity type.
        /// </summary>
        protected abstract BaseEntity newEntity();

        /// <summary>
        /// Creates the SQL INSERT statement for a given entity.
        /// </summary>
        protected abstract string CreateInsertOleDb(BaseEntity entity);

        /// <summary>
        /// Creates the SQL UPDATE statement for a given entity.
        /// </summary>
        protected abstract string CreateUpdateOleDb(BaseEntity entity);

        /// <summary>
        /// Creates the SQL DELETE statement for a given entity.
        /// </summary>
        protected abstract string CreateDeleteOleDb(BaseEntity entity);

        /// <summary>
        /// Maps an entity using the current reader state.
        /// </summary>
        protected abstract BaseEntity CreateModel(BaseEntity entity);

        /// <summary>
        /// Initializes the database connection and command.
        /// </summary>
        public BaseDB()
        {
            // Replace path below as needed
            connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\GitHub\FootballProject\FootballServerGood\DataAccess\Football2.accdb;Persist Security Info=True";
            connection = new OleDbConnection(connectionString);
            command = new OleDbCommand();
        }

        /// <summary>
        /// Executes a SELECT query and returns a list of entities.
        /// </summary>
        protected async Task<List<BaseEntity>> Select(string query)
        {
            List<BaseEntity> list = new List<BaseEntity>();

            try
            {
                command.Connection = connection;
                command.CommandText = query;
                connection.Open();
                reader = (OleDbDataReader)await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    BaseEntity entity = newEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\nOleDb: " + command.CommandText);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Adds an entity to the insert queue.
        /// </summary>
        public virtual void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = this.newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                inserted.Add(new ChangeEntity(this.CreateInsertOleDb, entity));
            }
        }

        /// <summary>
        /// Adds an entity to the update queue.
        /// </summary>
        public virtual void Update(BaseEntity entity)
        {
            BaseEntity reqEntity = this.newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(this.CreateUpdateOleDb, entity));
            }
        }

        /// <summary>
        /// Adds an entity to the delete queue.
        /// </summary>
        public virtual void Delete(BaseEntity entity)
        {
            BaseEntity reqEntity = this.newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                deleted.Add(new ChangeEntity(this.CreateDeleteOleDb, entity));
            }
        }

        /// <summary>
        /// Executes all pending INSERT, UPDATE, and DELETE operations.
        /// </summary>
        /// <returns>Total number of records affected.</returns>
        public async Task<int> SaveChanges()
        {
            int records_affected = 0;
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                cmd.Connection = this.connection;
                this.connection.Open();

                foreach (var item in inserted)
                {
                    cmd.CommandText = item.CreateOleDb(item.Entity);
                    records_affected += await cmd.ExecuteNonQueryAsync();

                    command.CommandText = "Select @@Identity";
                    item.Entity.Id = (int)command.ExecuteScalarAsync().Result;
                }
                foreach (var item in updated)
                {
                    cmd.CommandText = item.CreateOleDb(item.Entity);
                    records_affected += await cmd.ExecuteNonQueryAsync();
                }
                foreach (var item in deleted)
                {
                    cmd.CommandText = item.CreateOleDb(item.Entity);
                    records_affected += await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\nOleDb: " + cmd.CommandText);
            }
            finally
            {
                inserted.Clear();
                updated.Clear();
                deleted.Clear();

                if (connection.State == System.Data.ConnectionState.Open) connection.Close();
            }
            return records_affected;
        }
    }
}
