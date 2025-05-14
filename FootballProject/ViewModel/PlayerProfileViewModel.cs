using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.System;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(Player), "player")]
    [QueryProperty(nameof(Source), "source")]
    public class PlayerProfileViewModel : ViewModelBase
    {
        private Player player;
        private ObservableCollection<Stat> playerStats;
        private readonly StatsDB statsDB;
        private readonly UserService userService;
        private string source;

        public PlayerProfileViewModel(UserService Service)
        {
            userService = Service;
            statsDB = new StatsDB();
            
            BackCommand = new Command(OnBackCommandExecuted);
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


        public async void LoadNigger()
        {
            await userService.initStats(player.Id, player.Position);
            player.Stats = userService.GetAllStats(Player.Position, Player.Id).Result;
            PlayerStats = new ObservableCollection<Stat>(player.Stats);
        }

        public ICommand BackCommand { get; }


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
                    LoadNigger();
                }
            }
        }

        private async void OnBackCommandExecuted()
        {
            switch (Source)
            {
                case "ClubPlayersSearch":
                    await Shell.Current.GoToAsync("///rClubPlayersSearch");
                    break;
                case "PlayersSearch":
                default:
                    await Shell.Current.GoToAsync("///rPlayersSearch");
                    break;
            }
        }
    }
}