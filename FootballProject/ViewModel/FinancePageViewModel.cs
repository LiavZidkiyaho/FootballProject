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
    public class FinancePageViewModel : ViewModelBase
    {
        private readonly IUser webService;

        public Chart YearlyChart { get; private set; }
        public Chart MonthlyChart { get; private set; }

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

        public string TransferBudgetString => $"{TransferBudget:N0} €";
        public string WageString => $"{Wage:N0} €";
        public string TotalBalanceString => $"Total Balance: {TotalBalance:N0} €";
        public string TotalDiffString => $"Profit/Loss: {TotalDiff:N0} €";

        public ICommand SaveCommand { get; }

        public FinancePageViewModel(IUser service)
        {
            webService = service;
            SaveCommand = new Command(async () => await SaveNewBudget());

            YearlyChart = EmptyChart("No yearly data");
            MonthlyChart = EmptyChart("No monthly data");

            int teamId = webService.GetCurrentUser()?.Team.Id ?? 0;
            LoadBudgetData(teamId);
        }

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

                totalBudget = TransferBudget + Wage;
                SliderValue = totalBudget > 0 ? (int)Math.Round((double)TransferBudget * 100 / totalBudget) : 0;

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

        private void UpdateBudgetsBasedOnSlider()
        {
            if (totalBudget <= 0) return;

            long exactTransfer = (long)Math.Round(totalBudget * sliderValue / 100.0);
            long exactWage = totalBudget - exactTransfer;

            TransferBudget = exactTransfer;
            Wage = exactWage;

            OnPropertyChanged(nameof(TransferBudgetString));
            OnPropertyChanged(nameof(WageString));
        }

        private async Task SaveNewBudget()
        {
            try
            {
                var currentUser = webService.GetCurrentUser();
                if (currentUser == null) return;

                var newBudget = new Budget
                {
                    TeamId = currentUser.Team.Id,
                    Total = 1000,
                    EnterDate = DateTime.Now,
                    Purpose = "Transfer"
                };

                await webService.CreateBudget(newBudget);
                LoadBudgetData(currentUser.Team.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving budget: {ex.Message}");
            }
        }

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
