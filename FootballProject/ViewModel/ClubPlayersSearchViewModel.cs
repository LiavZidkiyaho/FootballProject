using FootballProject.Model;
using FootballProject.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    /// <summary>
    /// ViewModel for managing and searching players within the current user's club.
    /// Supports filtering and navigating to individual player profiles.
    /// </summary>
    public class ClubPlayersSearchViewModel : ViewModelBase
    {
        private readonly IUser userService;
        private ObservableCollection<Player> players;
        private string selectedField;
        private string filterValue;
        private string sortOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubPlayersSearchViewModel"/> class.
        /// </summary>
        /// <param name="service">An instance of the IUser service to access player and user data.</param>
        public ClubPlayersSearchViewModel(IUser service)
        {
            players = new ObservableCollection<Player>();
            userService = service;

            SelectedField = "Any";
            SortOrder = "None";

            SearchCommand = new Command(async () => await SearchPlayersAsync());
            NavigateToPlayerProfileCommand = new Command<Player>(async (player) => await NavigateToPlayerProfileAsync(player));
        }

        /// <summary>
        /// Gets or sets the collection of players belonging to the current user's team.
        /// </summary>
        public ObservableCollection<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected field used for filtering (currently unused).
        /// </summary>
        public string SelectedField
        {
            get => selectedField;
            set
            {
                selectedField = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the filter value to search players by name.
        /// </summary>
        public string FilterValue
        {
            get => filterValue;
            set
            {
                filterValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the sort order (e.g., ascending or descending — currently unused).
        /// </summary>
        public string SortOrder
        {
            get => sortOrder;
            set
            {
                sortOrder = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the command used to trigger a player search within the current team.
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// Gets the command used to navigate to a selected player's profile page.
        /// </summary>
        public ICommand NavigateToPlayerProfileCommand { get; }

        /// <summary>
        /// Searches for players within the current user's team, filtering by first name if specified.
        /// </summary>
        private async Task SearchPlayersAsync()
        {
            var manager = userService.GetCurrentUser();
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

        /// <summary>
        /// Navigates to the selected player's profile page.
        /// </summary>
        /// <param name="player">The player to view.</param>
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
