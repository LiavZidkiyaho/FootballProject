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
    /// ViewModel for searching and navigating through the list of all players (not limited to a single team).
    /// </summary>
    public class PlayersSearchViewModel : ViewModelBase
    {
        private readonly IUser service;

        private ObservableCollection<Player> players;
        private string selectedField;
        private string filterValue;
        private string sortOrder;

        /// <summary>
        /// Constructor initializes commands and default values.
        /// </summary>
        public PlayersSearchViewModel(IUser webService)
        {
            service = webService;

            players = new ObservableCollection<Player>();
            SelectedField = "Any";
            SortOrder = "None";

            SearchCommand = new Command(async () => await SearchPlayersAsync());
            NavigateToPlayerProfileCommand = new Command<Player>(async (player) => await NavigateToPlayerProfileAsync(player));
        }

        /// <summary>
        /// Collection of players displayed in the UI.
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
        /// The field selected for filtering or sorting players (e.g., "Position", "Nationality").
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
        /// The value to filter players by (e.g., "Goalkeeper").
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
        /// The order to sort players (e.g., "Ascending", "Descending").
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
        /// Command that executes a player search based on selected filters and sort options.
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// Command that navigates to a player's profile page.
        /// </summary>
        public ICommand NavigateToPlayerProfileCommand { get; }

        /// <summary>
        /// Performs the player search depending on the selected field, filter value, and sort order.
        /// </summary>
        private async Task SearchPlayersAsync()
        {
            if (SelectedField == "Any" || string.IsNullOrEmpty(SelectedField))
            {
                if (!string.IsNullOrWhiteSpace(FilterValue))
                {
                    Players = new ObservableCollection<Player>(
                        await service.SelectByFilter("Any", FilterValue));
                }
                else if (!string.IsNullOrWhiteSpace(SortOrder) && SortOrder != "None")
                {
                    Players = new ObservableCollection<Player>(
                        await service.SelectAndSort("Any", SortOrder));
                }
                else
                {
                    Players = new ObservableCollection<Player>(
                        await service.SelectAllPlayers());
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(FilterValue))
                {
                    Players = new ObservableCollection<Player>(
                        await service.SelectByFilter(SelectedField, FilterValue));
                }
                else if (!string.IsNullOrWhiteSpace(SortOrder) && SortOrder != "None")
                {
                    Players = new ObservableCollection<Player>(
                        await service.SelectAndSort(SelectedField, SortOrder));
                }
                else
                {
                    Players = new ObservableCollection<Player>(
                        await service.SelectAllPlayers());
                }
            }
        }

        /// <summary>
        /// Navigates to the player profile page with the selected player's data.
        /// </summary>
        /// <param name="player">The player to show details for.</param>
        private async Task NavigateToPlayerProfileAsync(Player player)
        {
            var data = new Dictionary<string, object>
            {
                { "player", player },
                { "source", "PlayersSearch" }
            };

            await Shell.Current.GoToAsync("/rPlayerProfile", data);
        }
    }
}
