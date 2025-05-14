using FootballServerGood.Model;

namespace FootballServerGood.DataAccess
{
    public class YearlyBudgetDB : BaseDB
    {
        protected override BaseEntity newEntity() => new Budget(); // or a specific model if needed

        protected override string CreateUpdateOleDb(BaseEntity entity)
        {
            var budget = (Budget)entity;

            return $@"
            UPDATE YearlyBudget
            SET 
                [1] = {budget.One},
                [2] = {budget.Two},
                [3] = {budget.Three},
                [4] = {budget.Four},
                [5] = {budget.Five}
            WHERE id = {budget.Total}";
        }

        protected override BaseEntity CreateModel(BaseEntity entity) => entity;
        protected override string CreateInsertOleDb(BaseEntity entity) => throw new NotImplementedException();
        protected override string CreateDeleteOleDb(BaseEntity entity) => throw new NotImplementedException();

        public async Task UpdateBudget(Budget budget)
        {
            Update(budget);         // queue the update
            await SaveChanges();    // save all queued changes
        }
    }
}
