using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    public class ClubPlayersSearchViewModel : ViewModelBase
    {
        private readonly IUser userService;
        private ObservableCollection<Player> players;
        private string selectedField;
        private string filterValue;
        private string sortOrder;

        public ClubPlayersSearchViewModel(IUser service)
        {
            players = new ObservableCollection<Player>();
            userService = service;

            SelectedField = "Any";
            SortOrder = "None";

            SearchCommand = new Command(async () => await SearchPlayersAsync());
            NavigateToPlayerProfileCommand = new Command<Player>(async (player) => await NavigateToPlayerProfileAsync(player));
        }

        public ObservableCollection<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged();
            }
        }

        public string SelectedField
        {
            get => selectedField;
            set
            {
                selectedField = value;
                OnPropertyChanged();
            }
        }

        public string FilterValue
        {
            get => filterValue;
            set
            {
                filterValue = value;
                OnPropertyChanged();
            }
        }

        public string SortOrder
        {
            get => sortOrder;
            set
            {
                sortOrder = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand NavigateToPlayerProfileCommand { get; }

        private async Task SearchPlayersAsync()
        {
            var manager = userService.GetCurrentUser(); // Only if local UserService
            if (manager?.Team == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No team associated with this user.", "OK");
                return;
            }

            var teamId = manager.Team.Id;

            if (!string.IsNullOrWhiteSpace(FilterValue))
            {
                Players = new ObservableCollection<Player>(
                    await userService.SelectTeamPlayersByFirstName(teamId, FilterValue));
            }
            else
            {
                Players = new ObservableCollection<Player>(
                    await userService.SelectPlayersByTeam(teamId));
            }
        }

        private async Task NavigateToPlayerProfileAsync(Player player)
        {
            var data = new Dictionary<string, object>
            {
                { "player", player },
                { "source", "ClubPlayersSearch" }
            };

            await Shell.Current.GoToAsync("/rPlayerProfile", data);
        }
    }
}
