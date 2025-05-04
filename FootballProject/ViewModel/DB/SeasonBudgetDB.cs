using FootballProject.Model;
using FootballProject.ViewModel.DB;

public class SeasonBudgetDB : BaseDB
{
    protected override BaseEntity newEntity() => new Budget();

    protected override string CreateUpdateOleDb(BaseEntity entity)
    {
        var budget = (Budget)entity;

        return $@"
            UPDATE SeasonBudget
            SET 
                [Jan] = {budget.Jan}, [Feb] = {budget.Feb}, [Mar] = {budget.Mar}, [Apr] = {budget.Apr},
                [June] = {budget.June}, [July] = {budget.July}, [Aug] = {budget.Aug}, [Sep] = {budget.Sep},
                [Oct] = {budget.Oct}, [Nov] = {budget.Nov}, [Dec] = {budget.Dec}
            WHERE id = {budget.ProfitLose}";
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
