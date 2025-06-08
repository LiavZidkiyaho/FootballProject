using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballServerGood.Model;

namespace FootballServerGood.DataAccess
{
    /// <summary>
    /// Data access layer for Budget entities. Inherits shared logic from BaseDB and implements budget-specific operations.
    /// </summary>
    public class BudgetDB : BaseDB
    {
        /// <summary>
        /// Creates a new empty Budget entity.
        /// </summary>
        protected override BaseEntity newEntity()
        {
            return new Budget();
        }

        /// <summary>
        /// Maps a database row to a Budget object using the current OleDbDataReader.
        /// </summary>
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var budget = (Budget)entity;

            budget.Id = reader.GetInt32(reader.GetOrdinal("id"));
            budget.TeamId = reader.GetInt32(reader.GetOrdinal("TeamId"));
            budget.Total = reader.GetInt32(reader.GetOrdinal("Total"));
            budget.EnterDate = reader.GetDateTime(reader.GetOrdinal("EnterDate"));
            budget.Purpose = reader.GetString(reader.GetOrdinal("Purpose"));

            return budget;
        }

        /// <summary>
        /// Generates the SQL INSERT command for a Budget.
        /// </summary>
        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;

            return $@"
INSERT INTO Budget (TeamId, Total, EnterDate, Purpose)
VALUES ({budget.TeamId}, {budget.Total}, #{budget.EnterDate:yyyy-MM-dd}#, '{budget.Purpose.Replace("'", "''")}')";
        }

        /// <summary>
        /// Generates the SQL UPDATE command for a Budget.
        /// </summary>
        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;

            return $@"
UPDATE Budget 
SET 
    TeamId = {budget.TeamId}, 
    Total = {budget.Total}, 
    EnterDate = #{budget.EnterDate:yyyy-MM-dd}#, 
    Purpose = '{budget.Purpose.Replace("'", "''")}'
WHERE id = {budget.Id}";
        }

        /// <summary>
        /// Generates the SQL DELETE command for a Budget.
        /// </summary>
        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;
            return $"DELETE FROM Budget WHERE id = {budget.Id}";
        }

        /// <summary>
        /// Returns a Budget object by its ID.
        /// </summary>
        /// <param name="Id">Budget ID</param>
        public async Task<Budget> SelectById(int Id)
        {
            string query = $"SELECT * FROM Budget WHERE Id = {Id}";
            List<BaseEntity> list = await base.Select(query);
            return list.FirstOrDefault() as Budget;
        }

        /// <summary>
        /// Returns all Budget records for a specific team.
        /// </summary>
        /// <param name="teamId">Team ID</param>
        public async Task<List<Budget>> SelectByTeamId(int teamId)
        {
            string query = $"SELECT * FROM Budget WHERE TeamId = {teamId}";
            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Budget>().ToList();
        }

        /// <summary>
        /// Updates an existing budget entry in the database.
        /// </summary>
        public async Task UpdateBudget(Budget budget)
        {
            Update(budget);         // queue the update
            await SaveChanges();    // save all queued changes
        }

        /// <summary>
        /// Inserts a new budget entry into the database.
        /// </summary>
        public async Task InsertBudget(Budget budget)
        {
            Insert(budget);         // queue the insert
            await SaveChanges();    // save all queued changes
        }

        /// <summary>
        /// Deletes a budget entry from the database.
        /// </summary>
        public async Task DeleteBudget(Budget budget)
        {
            Delete(budget);         // queue the deletion
            await SaveChanges();    // save all queued changes
        }
    }
}
