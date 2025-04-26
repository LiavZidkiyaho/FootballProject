using System;
using System.Collections.Generic;
using System.Data.OleDb;
using FootballProject.Model;

namespace FootballProject.ViewModel.DB
{
    public abstract class BaseDB
    {
        private string connectionString;
        protected OleDbConnection connection;
        protected OleDbCommand command;
        protected OleDbDataReader reader;

        protected List<ChangeEntity> inserted = new List<ChangeEntity>();
        protected List<ChangeEntity> deleted = new List<ChangeEntity>();
        protected List<ChangeEntity> updated = new List<ChangeEntity>();

        protected abstract BaseEntity newEntity();
        protected abstract string CreateInsertOleDb(BaseEntity entity);
        protected abstract string CreateUpdateOleDb(BaseEntity entity);
        protected abstract string CreateDeleteOleDb(BaseEntity entity);
        protected abstract BaseEntity CreateModel(BaseEntity entity);

        public BaseDB()
        {
            //connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Users\User\Documents\GitHub\FootballProject\FootballProject\ViewModel\DB\Football2.accdb;Persist Security Info=True";
            connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\אבי\Documents\GitHub\FootballProject\FootballProject\ViewModel\DB\Football2.accdb;Persist Security Info=True";
            connection = new OleDbConnection(connectionString);
            command = new OleDbCommand();
        }

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

        public virtual void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = this.newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                inserted.Add(new ChangeEntity(this.CreateInsertOleDb, entity));
            }
        }

        public virtual void Update(BaseEntity entity)
        {
            BaseEntity reqEntity = this.newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(this.CreateUpdateOleDb, entity));
            }
        }

        public virtual void Delete(BaseEntity entity)
        {
            BaseEntity reqEntity = this.newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                deleted.Add(new ChangeEntity(this.CreateDeleteOleDb, entity));
            }
        }

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