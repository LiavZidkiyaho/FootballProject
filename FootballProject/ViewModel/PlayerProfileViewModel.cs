using FootballProject.Model;
using FootballProject.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(Player), "player")]
    [QueryProperty(nameof(Source), "source")]
    public class PlayerProfileViewModel : ViewModelBase
    {
        private Player player;
        private ObservableCollection<Stat> playerStats;
        private readonly IUser userService;
        private string source;

        public PlayerProfileViewModel(IUser service)
        {
            userService = service;

            BackCommand = new Command(async () => await OnBackCommandExecuted());
        }

        public ObservableCollection<Stat> PlayerStats
        {
            get => playerStats;
            set
            {
                playerStats = value;
                OnPropertyChanged();
            }
        }

        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged();
            }
        }

        public Player Player
        {
            get => player;
            set
            {
                player = value;
                OnPropertyChanged();

                if (player != null)
                {
                    _ = LoadPlayerStatsAsync(); // fire-and-forget
                }
            }
        }

        public ICommand BackCommand { get; }

        private async Task LoadPlayerStatsAsync()
        {
            var stats = await userService.GetAllStats(player.Position, player.Id);
            player.Stats = stats;
            PlayerStats = new ObservableCollection<Stat>(stats);
        }

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
