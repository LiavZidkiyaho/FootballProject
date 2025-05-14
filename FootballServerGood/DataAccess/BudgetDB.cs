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
            budget.Wage = reader.GetInt32(reader.GetOrdinal("Wage"));
            budget.Transfer = reader.GetInt32(reader.GetOrdinal("Transfer"));

            budget.YearId = reader.GetInt32(reader.GetOrdinal("Total"));
            budget.SeasonId = reader.GetInt32(reader.GetOrdinal("Profit/Lose"));

            // YearlyBudget fields
            budget.One = reader.GetInt32(reader.GetOrdinal("1"));
            budget.Two = reader.GetInt32(reader.GetOrdinal("2"));
            budget.Three = reader.GetInt32(reader.GetOrdinal("3"));
            budget.Four = reader.GetInt32(reader.GetOrdinal("4"));
            budget.Five = reader.GetInt32(reader.GetOrdinal("5"));

            // SeasonBudget fields
            budget.Jan = reader.GetInt32(reader.GetOrdinal("Jan"));
            budget.Feb = reader.GetInt32(reader.GetOrdinal("Feb"));
            budget.Mar = reader.GetInt32(reader.GetOrdinal("Mar"));
            budget.Apr = reader.GetInt32(reader.GetOrdinal("Apr"));
            budget.June = reader.GetInt32(reader.GetOrdinal("June"));
            budget.July = reader.GetInt32(reader.GetOrdinal("July"));
            budget.Aug = reader.GetInt32(reader.GetOrdinal("Aug"));
            budget.Sep = reader.GetInt32(reader.GetOrdinal("Sep"));
            budget.Oct = reader.GetInt32(reader.GetOrdinal("Oct"));
            budget.Nov = reader.GetInt32(reader.GetOrdinal("Nov"));
            budget.Dec = reader.GetInt32(reader.GetOrdinal("Dec"));

            budget.ProfitLose = budget.Jan + budget.Feb + budget.Mar + budget.Apr + budget.June + budget.July + budget.Aug + budget.Sep + budget.Oct + budget.Nov + budget.Dec;
            budget.Total = budget.One + budget.Two + budget.Three + budget.Four + budget.Five;

            return budget;
        }

        protected override string CreateInsertOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;

            return $@"
        UPDATE Budget 
        SET 
            [Wage] = {budget.Wage},
            [Transfer] = {budget.Transfer},
            [Total] = {budget.Total},
            [Profit/Lose] = {budget.ProfitLose}
        WHERE TeamId = {budget.TeamId}";
        }


        protected override string CreateDeleteOleDb(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Budget> SelectByTeamId(int teamId)
        {
            string query = @"
SELECT 
    b.*, 
    y.[1], y.[2], y.[3], y.[4], y.[5],
    s.[Jan], s.[Feb], s.[Mar], s.[Apr], s.[June], s.[July], s.[Aug], s.[Sep], s.[Oct], s.[Nov], s.[Dec],
    b.[Profit/Lose] AS ProfitLose
FROM 
    ((Budget AS b 
    INNER JOIN YearlyBudget AS y ON b.Total = y.[id])
    INNER JOIN SeasonBudget AS s ON b.[Profit/Lose] = s.[id])
WHERE 
    b.TeamId = " + teamId;




            List<BaseEntity> list = await base.Select(query);
            return list.FirstOrDefault() as Budget;
        }

        public async Task UpdateBudget(Budget budget)
        {
            Update(budget);         // queue the update
            await SaveChanges();    // save all queued changes
        }
    }
}
