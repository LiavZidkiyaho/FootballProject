using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;
using Microcharts;
using SkiaSharp;

namespace FootballProject.ViewModel
{
    public class FinancePageViewModel : ViewModelBase
    {
        private readonly BudgetDB budgetDB;
        private readonly YearlyBudgetDB yearlyBudgetDB;
        private readonly SeasonBudgetDB seasonBudgetDB;
        private long bankBalance;
        private readonly UserService userService;
        public Chart YearlyChart { get; private set; }
        public Chart MonthlyChart { get; private set; }

        public long BankBalance
        {
            get => bankBalance;
            set { bankBalance = value; OnPropertyChanged(); OnPropertyChanged(nameof(BankBalanceString)); }
        }

        private long profitOrLoss;
        public long ProfitOrLoss
        {
            get => profitOrLoss;
            set { profitOrLoss = value; OnPropertyChanged(); OnPropertyChanged(nameof(ProfitOrLossString)); }
        }

        private long transferBudget;
        public long TransferBudget
        {
            get => transferBudget;
            set
            {
                transferBudget = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TransferBudgetString));
            }
        }

        private long wage;
        public long Wage
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

        private long totalBudget;

        public int One { get; set; }
        public int Two { get; set; }
        public int Three { get; set; }
        public int Four { get; set; }
        public int Five { get; set; }

        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }

        public int YearlyId { get; set; } // Add a computed property or bind this as needed
        public int SeasonId { get; set; } // Add a computed property or bind this as needed

        public string BankBalanceString => $"Overall Bank Balance: {BankBalance:N0} €";
        public string ProfitOrLossString => $"Profit/Loss This Season: {ProfitOrLoss:N0} €";
        public string TransferBudgetString => $"{TransferBudget:N0} €";
        public string WageString => $"{Wage:N0} €";

        public FinancePageViewModel(UserService service)
        {
            userService = service;
            budgetDB = new BudgetDB();
            yearlyBudgetDB = new YearlyBudgetDB();
            seasonBudgetDB = new SeasonBudgetDB();
            int teamId = userService.GetCurrentUser().Team.Id;
            LoadBudgetData(teamId);
            SaveCommand = new Command(async () => await SaveBudgets());
            YearlyChart = new LineChart
            {
                Entries = new[]
                {
                    new ChartEntry(20) { Label = "No Data", ValueLabel = "0", Color = SKColors.Gray }
                }
            };
            MonthlyChart = new LineChart
            {
                Entries = new[]
               {
                    new ChartEntry(20) { Label = "No Data", ValueLabel = "0", Color = SKColors.Gray }
                }
            };
            LoadYearlyChart();
            LoadMonthlyChart();
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
                    YearlyId = budget.YearId;
                    SeasonId = budget.SeasonId;

                    One = budget.One;
                    Two = budget.Two;
                    Three = budget.Three;
                    Four = budget.Four;
                    Five = budget.Five;

                    Jan = budget.Jan;
                    Feb = budget.Feb;
                    Mar = budget.Mar;
                    Apr = budget.Apr;
                    June = budget.June;
                    July = budget.July;
                    Aug = budget.Aug;
                    Sep = budget.Sep;
                    Oct = budget.Oct;
                    Nov = budget.Nov;
                    Dec = budget.Dec;

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
                BankBalance = -1;
                ProfitOrLoss = -1;
                TransferBudget = -1;
                Wage = -1;
                totalBudget = -1;
                SliderValue = 0;
            }
        }


        private void LoadYearlyChart()
        {
            YearlyChart = new LineChart
            {
                Entries = new[]
                {
                    new ChartEntry(One) { Label = "Year 1", ValueLabel = One.ToString(), Color = SKColors.Red },
                    new ChartEntry(Two) { Label = "Year 2", ValueLabel = Two.ToString(), Color = SKColors.Orange },
                    new ChartEntry(Three) { Label = "Year 3", ValueLabel = Three.ToString(), Color = SKColors.Yellow },
                    new ChartEntry(Four) { Label = "Year 4", ValueLabel = Four.ToString(), Color = SKColors.Green },
                    new ChartEntry(Five) { Label = "Year 5", ValueLabel = Five.ToString(), Color = SKColors.Blue }
                },
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };

            OnPropertyChanged(nameof(YearlyChart));
        }

        private void LoadMonthlyChart()
        {
            MonthlyChart = new LineChart
            {
                Entries = new[]
                {
                    new ChartEntry(Jan) { Label = "Jan", ValueLabel = Jan.ToString(), Color = SKColors.Red },
                    new ChartEntry(Feb) { Label = "Feb", ValueLabel = Feb.ToString(), Color = SKColors.Orange },
                    new ChartEntry(Mar) { Label = "Mar", ValueLabel = Mar.ToString(), Color = SKColors.Yellow },
                    new ChartEntry(Apr) { Label = "Apr", ValueLabel = Apr.ToString(), Color = SKColors.Green },
                    new ChartEntry(June) { Label = "Jun", ValueLabel = June.ToString(), Color = SKColors.Teal },
                    new ChartEntry(July) { Label = "Jul", ValueLabel = July.ToString(), Color = SKColors.Blue },
                    new ChartEntry(Aug) { Label = "Aug", ValueLabel = Aug.ToString(), Color = SKColors.Purple },
                    new ChartEntry(Sep) { Label = "Sep", ValueLabel = Sep.ToString(), Color = SKColors.Magenta },
                    new ChartEntry(Oct) { Label = "Oct", ValueLabel = Oct.ToString(), Color = SKColors.Brown },
                    new ChartEntry(Nov) { Label = "Nov", ValueLabel = Nov.ToString(), Color = SKColors.Gray },
                    new ChartEntry(Dec) { Label = "Dec", ValueLabel = Dec.ToString(), Color = SKColors.Black }
                },
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
            OnPropertyChanged(nameof(MonthlyChart));
        }


        private void UpdateBudgetsBasedOnSlider()
        {
            if (totalBudget <= 0) return;

            System.Diagnostics.Debug.WriteLine($"slider value: {sliderValue}");

            TransferBudget = (int)(totalBudget * ((float)sliderValue / 100f));
            Wage = totalBudget - TransferBudget;

            System.Diagnostics.Debug.WriteLine($"TB value: {TransferBudget}");
            System.Diagnostics.Debug.WriteLine($"Wage value: {Wage}");

            OnPropertyChanged(nameof(TransferBudgetString));
            OnPropertyChanged(nameof(WageString));
        }

        private async Task SaveBudgets()
        {
            if (totalBudget <= 0) return;

            try
            {
                var currentUser = userService.GetCurrentUser();

                Budget updatedBudget = new Budget
                {
                    TeamId = currentUser.Team.Id,
                    Transfer = this.TransferBudget,
                    Wage = this.Wage,
                    Total = this.YearlyId,
                    ProfitLose = this.SeasonId,

                    One = this.One,
                    Two = this.Two,
                    Three = this.Three,
                    Four = this.Four,
                    Five = this.Five,

                    Jan = this.Jan,
                    Feb = this.Feb,
                    Mar = this.Mar,
                    Apr = this.Apr,
                    June = this.June,
                    July = this.July,
                    Aug = this.Aug,
                    Sep = this.Sep,
                    Oct = this.Oct,
                    Nov = this.Nov,
                    Dec = this.Dec
                };

                await budgetDB.UpdateBudget(updatedBudget);
                await yearlyBudgetDB.UpdateBudget(updatedBudget);
                await seasonBudgetDB.UpdateBudget(updatedBudget);

                await budgetDB.SaveChanges();
                await yearlyBudgetDB.SaveChanges();
                await seasonBudgetDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving budget: {ex.Message}");
            }
        }
    }
}
