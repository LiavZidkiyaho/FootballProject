using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;

namespace FootballProject.ViewModel
{
    public class FinancePageViewModel : ViewModelBase
    {
        // Raw numeric values for calculations (use long to handle large budgets)
        private readonly BudgetDB budgetDB;
        private int bankBalance;
        private readonly UserService userService;
        public int BankBalance
        {
            get => bankBalance;
            set { bankBalance = value; OnPropertyChanged(); OnPropertyChanged(nameof(BankBalanceString)); }
        }

        private int profitOrLoss;
        public int ProfitOrLoss
        {
            get => profitOrLoss;
            set { profitOrLoss = value; OnPropertyChanged(); OnPropertyChanged(nameof(ProfitOrLossString)); }
        }

        private int transferBudget;
        public int TransferBudget
        {
            get => transferBudget;
            set
            {
                transferBudget = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TransferBudgetString));
            }
        }

        private int wage;
        public int Wage
        {
            get => wage;
            set
            {
                wage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WageString));
            }
        }

        private int sliderValue;
        public int SliderValue
        {
            get => sliderValue;
            set
            {
                sliderValue = value;
                UpdateBudgetsBasedOnSlider();
                OnPropertyChanged();
            }
        }

        private int totalBudget; // Total budget remains constant

        // String properties for formatted display
        public string BankBalanceString => $"Overall Bank Balance: {BankBalance:N0} €";
        public string ProfitOrLossString => $"Profit/Loss This Season: {ProfitOrLoss:N0} €";
        public string TransferBudgetString => $"{TransferBudget:N0} €";
        public string WageString => $"{Wage:N0} €"; 

        public FinancePageViewModel(UserService service)
        {
            userService = service;
            budgetDB = new BudgetDB();
            int teamId = userService.GetCurrentUser().Team.Id; // Adjust path to team ID if needed
            LoadBudgetData(teamId);
            SaveCommand = new Command(async () => await SaveBudgets());
        }

        public ICommand SaveCommand { get; }

        private async void LoadBudgetData(int teamId)
        {
            try
            {
                Budget budget = await budgetDB.SelectByTeamId(teamId);

                if (budget != null)
                {
                    BankBalance = budget.Total;
                    ProfitOrLoss = budget.ProfitLose;
                    TransferBudget = budget.Transfer;
                    Wage = budget.Wage;

                    totalBudget = TransferBudget + Wage;
                    SliderValue = (int)((TransferBudget * 100) / totalBudget);

                }
                else
                {
                    BankBalance = 0;
                    ProfitOrLoss = 0;
                    TransferBudget = 0;
                    Wage = 0;
                    totalBudget = 0;
                    SliderValue = 0;
                }
            }
            catch (Exception)
            {
                BankBalance = -1; // Indicate error state
                ProfitOrLoss = -1;
                TransferBudget = -1;
                Wage = -1;
                totalBudget = -1;
                SliderValue = 0;
            }
        }

        private void UpdateBudgetsBasedOnSlider()
        {
            if (totalBudget <= 0) return;

            System.Diagnostics.Debug.WriteLine($"slider value: {sliderValue}");

            // Correct formula:
            TransferBudget = (int)(totalBudget * ((float)sliderValue / ((float)100)));
            System.Diagnostics.Debug.WriteLine($"TB value: {TransferBudget}");
            Wage = totalBudget - TransferBudget;
            System.Diagnostics.Debug.WriteLine($"Wage value: {Wage}");


            // Update UI
            OnPropertyChanged(nameof(TransferBudgetString));
            OnPropertyChanged(nameof(WageString));
        }




        private async Task SaveBudgets()
        {
            if (totalBudget <= 0) return;

            try
            {
                // Create a Budget object with current values
                Budget updatedBudget = new Budget  {};

                updatedBudget.Transfer = this.TransferBudget;
                updatedBudget.Wage = this.Wage;
                updatedBudget.Total = this.BankBalance;
                updatedBudget.ProfitLose = this.ProfitOrLoss;
                updatedBudget.TeamId = userService.GetCurrentUser().Team.Id;

                await budgetDB.UpdateBudget(updatedBudget);
            }
            catch (Exception ex)
            {
                // Handle errors (optional: show a popup message or log it)
                Console.WriteLine($"Error saving budget: {ex.Message}");
            }
        }

    }
}