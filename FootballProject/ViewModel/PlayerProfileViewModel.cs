using FootballProject.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(Player), "player")]
    public class PlayerProfileViewModel : ViewModelBase
    {
        private Player player;

        public PlayerProfileViewModel()
        {
            BackCommand = new Command(OnBackCommandExecuted);
        }
        public ICommand BackCommand { get; }

        public Player Player
        {
            get => player;
            set
            {
                player = value;
                OnPropertyChanged();
            }
        }

        private async void OnBackCommandExecuted()
        {
            await Shell.Current.GoToAsync("///rPlayersSearch");
        }
    }
}