using FootballProject.Model;
using FootballProject.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballProject.ViewModel
{
    /// <summary>
    /// ViewModel for displaying and managing the finance data of a team.
    /// Includes budget entry, budget visualization by year/month, and summary calculations.
    /// </summary>
    public class FinancePageViewModel : ViewModelBase
    {
        private readonly IUser webService;

        /// <summary>
        /// Chart displaying total balance per year (last 5 years).
        /// </summary>
        public Chart YearlyChart { get; private set; }

        /// <summary>
        /// Chart displaying monthly profit/loss over the past 12 months.
        /// </summary>
        public Chart MonthlyChart { get; private set; }

        // --- Budget Summary Properties ---

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

        private long totalBalance;
        public long TotalBalance
        {
            get => totalBalance;
            set
            {
                totalBalance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalBalanceString));
            }
        }

        private long totalDiff;
        public long TotalDiff
        {
            get => totalDiff;
            set
            {
                totalDiff = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalDiffString));
            }
        }

        /// <summary>
        /// Formatted transfer budget string.
        /// </summary>
        public string TransferBudgetString => $"{TransferBudget:N0} €";

        /// <summary>
        /// Formatted wage string.
        /// </summary>
        public string WageString => $"{Wage:N0} €";

        /// <summary>
        /// Formatted total balance string.
        /// </summary>
        public string TotalBalanceString => $"Total Balance: {TotalBalance:N0} €";

        /// <summary>
        /// Formatted profit/loss string.
        /// </summary>
        public string TotalDiffString => $"Profit/Loss: {TotalDiff:N0} €";

        // --- Form Input Properties ---

        /// <summary>
        /// List of available purposes for budget entries.
        /// </summary>
        public List<string> BudgetPurposes { get; } = new() { "Transfer", "Wage", "Balance", "Difference" };

        private string newBudgetAmount;
        /// <summary>
        /// New budget amount input (string).
        /// </summary>
        public string NewBudgetAmount
        {
            get => newBudgetAmount;
            set
            {
                newBudgetAmount = value;
                OnPropertyChanged();
            }
        }

        private string selectedPurpose;
        /// <summary>
        /// Selected purpose from the dropdown.
        /// </summary>
        public string SelectedPurpose
        {
            get => selectedPurpose;
            set
            {
                selectedPurpose = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command to save a new budget entry.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Constructor. Initializes command, charts, and triggers data loading.
        /// </summary>
        /// <param name="service">Injected IUser service.</param>
        public FinancePageViewModel(IUser service)
        {
            webService = service;
            SaveCommand = new Command(async () => await SaveNewBudget());

            YearlyChart = EmptyChart("No yearly data");
            MonthlyChart = EmptyChart("No monthly data");

            int teamId = webService.GetCurrentUser()?.Team.Id ?? 0;
            LoadBudgetData(teamId);
        }

        /// <summary>
        /// Loads all budget data for the team, updates totals and charts.
        /// </summary>
        /// <param name="teamId">Current user's team ID.</param>
        private async void LoadBudgetData(int teamId)
        {
            try
            {
                var allBudgets = await webService.GetBudgetsByTeamId(teamId) ?? new List<Budget>();
                var now = DateTime.Now;

                TransferBudget = allBudgets.Where(b => b.Purpose == "Transfer").Sum(b => b.Total);
                Wage = allBudgets.Where(b => b.Purpose == "Wage").Sum(b => b.Total);
                TotalBalance = allBudgets.Where(b => b.Purpose == "Balance").Sum(b => b.Total);
                TotalDiff = allBudgets.Where(b => b.Purpose == "Difference").Sum(b => b.Total);

                YearlyChart = CreateChart(allBudgets
                    .Where(b => b.Purpose == "Balance" && b.EnterDate.Year >= now.Year - 4)
                    .GroupBy(b => b.EnterDate.Year), SKColors.Green);

                MonthlyChart = CreateChart(allBudgets
                    .Where(b => b.Purpose == "Difference" && b.EnterDate >= now.AddMonths(-11))
                    .GroupBy(b => new { b.EnterDate.Year, b.EnterDate.Month })
                    .Select(g => new
                    {
                        Key = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                        Value = g.Sum(b => b.Total)
                    }), SKColors.Red);

                OnPropertyChanged(nameof(YearlyChart));
                OnPropertyChanged(nameof(MonthlyChart));
                OnPropertyChanged(nameof(TransferBudgetString));
                OnPropertyChanged(nameof(WageString));
                OnPropertyChanged(nameof(TotalBalanceString));
                OnPropertyChanged(nameof(TotalDiffString));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading finance data: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves a new budget entry for the current user's team.
        /// </summary>
        private async Task SaveNewBudget()
        {
            try
            {
                var currentUser = webService.GetCurrentUser();
                if (currentUser == null) return;

                if (!long.TryParse(NewBudgetAmount, out var amount) || string.IsNullOrEmpty(SelectedPurpose))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric amount and select a purpose.");
                    return;
                }

                var newBudget = new Budget
                {
                    TeamId = currentUser.Team.Id,
                    Total = amount,
                    EnterDate = DateTime.Now,
                    Purpose = SelectedPurpose
                };

                await webService.CreateBudget(newBudget);

                // Clear inputs
                NewBudgetAmount = string.Empty;
                SelectedPurpose = null;

                LoadBudgetData(currentUser.Team.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving budget: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a yearly or grouped chart from integer keys (e.g., year).
        /// </summary>
        private LineChart CreateChart(IEnumerable<IGrouping<int, Budget>> groupedData, SKColor color)
        {
            var entries = groupedData
                .OrderBy(g => g.Key)
                .Select(g => new ChartEntry(g.Sum(b => b.Total))
                {
                    Label = g.Key.ToString(),
                    ValueLabel = g.Sum(b => b.Total).ToString(),
                    Color = color
                }).ToList();

            return entries.Any()
                ? new LineChart { Entries = entries }
                : EmptyChart("No data");
        }

        /// <summary>
        /// Creates a chart from dynamic grouped values (e.g., monthly).
        /// </summary>
        private LineChart CreateChart(IEnumerable<dynamic> data, SKColor color)
        {
            var entries = data
                .OrderBy(d => d.Key)
                .Select(d => new ChartEntry(d.Value)
                {
                    Label = d.Key.ToString(),
                    ValueLabel = d.Value.ToString(),
                    Color = color
                }).ToList();

            return entries.Any()
                ? new LineChart { Entries = entries }
                : EmptyChart("No data");
        }

        /// <summary>
        /// Generates a fallback chart with a placeholder entry.
        /// </summary>
        private LineChart EmptyChart(string label)
        {
            return new LineChart
            {
                Entries = new List<ChartEntry>
                {
                    new ChartEntry(0) { Label = label, ValueLabel = "0", Color = SKColors.Gray }
                },
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
        }
    }
}
