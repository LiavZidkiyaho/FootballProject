using FootballProject.Model;
using System;
using System.Collections.Generic;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(Player), "player")]
    public class PlayerProfileViewModel : ViewModelBase
    {
        private Player player;

        public PlayerProfileViewModel()
        {
        }

        public Player Player
        {
            get => player;
            set
            {
                player = value;
                OnPropertyChanged();
            }
        }
    }
}