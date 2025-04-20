using System;
using System.Threading.Tasks;
using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;

namespace FootballProject.ViewModel
{
    public class FinancePageViewModel : ViewModelBase
    {
        private string bankBalance;
        public string BankBalance
        {
            get => bankBalance;
            set { bankBalance = value; OnPropertyChanged(); }
        }

        private string profitOrLoss;
        public string ProfitOrLoss
        {
            get => profitOrLoss;
            set { profitOrLoss = value; OnPropertyChanged(); }
        }

        private string transferBudget;
        public string TransferBudget
        {
            get => transferBudget;
            set { transferBudget = value; OnPropertyChanged(); }
        }

        private string wage;
        public string Wage
        {
            get => wage;
            set { wage = value; OnPropertyChanged(); }
        }

        private readonly BudgetDB budgetDB = new BudgetDB();

        public FinancePageViewModel(UserService service)
        {
            int teamId = service.GetCurrentUser().Team.Id; // Adjust path to team ID if needed
            LoadBudgetData(teamId);
        }

        private async void LoadBudgetData(int teamId)
        {
            try
            {
                
                Budget budget = await budgetDB.SelectByTeamId(teamId);

                if (budget != null)
                {
                    BankBalance = $"Overall Bank Balance: {budget.Total:N0} €";
                    ProfitOrLoss = $"Profit/Loss This Season: {budget.ProfitLose:N0} €";
                    TransferBudget = $"{budget.Transfer:N0} €";
                    Wage = $"{budget.Wage:N0} €";
                }
                else
                {
                    BankBalance = "Overall Bank Balance: N/A";
                    ProfitOrLoss = "Profit/Loss This Season: N/A";
                    TransferBudget = "N/A";
                    Wage = "N/A";
                }
            }
            catch (Exception ex)
            {
                BankBalance = "Error";
                ProfitOrLoss = "Error";
                TransferBudget = "Error";
                Wage = "Error";
            }
        }
    }
}
