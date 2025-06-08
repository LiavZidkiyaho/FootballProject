using FootballProject.Model;
using FootballProject.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace FootballProject.ViewModel
{
    /// <summary>
    /// ViewModel for displaying and evaluating a player's profile and rating.
    /// </summary>
    [QueryProperty(nameof(Player), "player")]
    [QueryProperty(nameof(Source), "source")]
    public class PlayerProfileViewModel : ViewModelBase
    {
        private Player player;
        private ObservableCollection<Stat> playerStats;
        private readonly IUser userService;
        private string source;
        private double playerRating;

        /// <summary>
        /// Initializes a new instance of the PlayerProfileViewModel class.
        /// </summary>
        /// <param name="service">Injected user service for accessing stats and navigation.</param>
        public PlayerProfileViewModel(IUser service)
        {
            userService = service;
            BackCommand = new Command(async () => await OnBackCommandExecuted());
        }

        /// <summary>
        /// Gets or sets the stats of the selected player.
        /// </summary>
        public ObservableCollection<Stat> PlayerStats
        {
            get => playerStats;
            set
            {
                playerStats = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Source page for determining navigation logic.
        /// </summary>
        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The player whose profile is being displayed.
        /// </summary>
        public Player Player
        {
            get => player;
            set
            {
                player = value;
                OnPropertyChanged();

                if (player != null)
                {
                    _ = LoadPlayerStatsAsync();
                }
            }
        }

        /// <summary>
        /// The calculated rating of the player (scaled 0-100).
        /// </summary>
        public double PlayerRating
        {
            get => playerRating;
            set
            {
                playerRating = Math.Round(value, 2);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command to navigate back to the previous page.
        /// </summary>
        public ICommand BackCommand { get; }

        /// <summary>
        /// Loads stats for the player and calculates their rating.
        /// </summary>
        private async Task LoadPlayerStatsAsync()
        {
            var stats = await userService.GetAllStats(player.Position, player.Id);
            player.Stats = stats;
            PlayerStats = new ObservableCollection<Stat>(stats);
            PlayerRating = CalculatePlayerRating(stats);
        }

        /// <summary>
        /// Returns a level (1–5) based on the value range.
        /// </summary>
        private int GetLevel(double value)
        {
            if (value < 25) return 1;
            else if (value < 50) return 2;
            else if (value < 75) return 3;
            else if (value < 100) return 4;
            else return 5;
        }

        /// <summary>
        /// Calculates a rating based on stat levels and penalizes negative stats.
        /// </summary>
        private double CalculatePlayerRating(List<Stat> stats)
        {
            var negativeStats = new HashSet<string>
            {
                "fouls_conceded", "yellow_cards", "red_cards", "turnovers",
                "defensive_errors", "possession_lost", "offsides", "big_chances_missed",
                "goals_conceded_per_90"
            };

            int rawScore = 0;
            int statCount = 0;

            foreach (var stat in stats)
            {
                if (stat == null) continue;

                double value = stat.Value;

                // Convert per_90 stats to full-game basis
                if (stat.Name.ToLower().EndsWith("_per_90"))
                {
                    value *= 90;
                }

                int level = GetLevel(value);
                statCount++;

                if (negativeStats.Contains(stat.Name.ToLower()))
                    rawScore -= level;
                else
                    rawScore += level;
            }

            int maxPossibleScore = statCount * 5;
            if (maxPossibleScore == 0) return 0;

            double scaledScore = (double)rawScore / maxPossibleScore * 100;
            return Math.Max(0, Math.Min(100, scaledScore));
        }

        /// <summary>
        /// Handles back navigation depending on source page.
        /// </summary>
        private async Task OnBackCommandExecuted()
        {
            switch (Source)
            {
                case "ClubPlayersSearch":
                    await Shell.Current.GoToAsync("///rClubPlayersSearch");
                    PlayerStats = null;
                    break;
                case "PlayersSearch":
                default:
                    await Shell.Current.GoToAsync("///rPlayersSearch");
                    PlayerStats = null;
                    break;
            }
        }
    }
}
