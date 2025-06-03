using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballServerGood.Model;

namespace FootballServerGood.DataAccess
{
    public class BudgetDB : BaseDB
    {
        protected override BaseEntity newEntity()
        {
            return new Budget();
        }

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

        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;

            return $@"
INSERT INTO Budget (TeamId, Total, EnterDate, Purpose)
VALUES ({budget.TeamId}, {budget.Total}, #{budget.EnterDate:yyyy-MM-dd}#, '{budget.Purpose.Replace("'", "''")}')";
        }

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

        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;
            return $"DELETE FROM Budget WHERE id = {budget.Id}";
        }

        public async Task<Budget> SelectById(int Id)
        {
            string query = $"SELECT * FROM Budget WHERE Id = {Id}";
            List<BaseEntity> list = await base.Select(query);
            return list.FirstOrDefault() as Budget;
        }

        public async Task<List<Budget>> SelectByTeamId(int teamId)
        {
            string query = $"SELECT * FROM Budget WHERE TeamId = {teamId}";
            List<BaseEntity> list = await base.Select(query);
            return list.Cast<Budget>().ToList();
        }

        public async Task UpdateBudget(Budget budget)
        {
            Update(budget);         // queue the update
            await SaveChanges();    // save all queued changes
        }

        public async Task InsertBudget(Budget budget)
        {
            Insert(budget);         // queue the insert
            await SaveChanges();    // save all queued changes
        }

        public async Task DeleteBudget(Budget budget)
        {
            Delete(budget);         // queue the deletion
            await SaveChanges();    // save all queued changes
        }
    }
}
