using System;
using System.Collections.Generic;
using System.Data;
using FootballProject.Model;

namespace FootballProject.ViewModel.DB
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

            budget.Id = reader.GetInt32(1);              // id
            budget.TeamId = reader.GetInt32(2);          // TeamId
            budget.Wage = reader.GetInt32(3);          // Wage
            budget.Total = reader.GetInt32(4);         // Total
            budget.ProfitLose = reader.GetInt32(0);    // Profit/Lose
            budget.Transfer = reader.GetInt32(5);      // Transfer

            return budget;
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


        public async Task<Budget> SelectByTeamId(int teamId)
        {
            string query = $"SELECT *, [Profit/Lose] AS ProfitLose FROM budget WHERE TeamId = {teamId}";
            List<BaseEntity> list = await base.Select(query);
            return list.FirstOrDefault() as Budget;
        }
    }
}
