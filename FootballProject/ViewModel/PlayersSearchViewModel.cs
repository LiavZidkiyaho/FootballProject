using FootballProject.Model;
using FootballProject.ViewModel.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace FootballProject.ViewModel
{
    public class PlayersSearchViewModel : ViewModelBase
    {
        private readonly PlayerDB playerDB;
        private ObservableCollection<Player> players;
        private string selectedField;
        private string filterValue;
        private string sortOrder;

        public PlayersSearchViewModel()
        {
            playerDB = new PlayerDB();
            players = new ObservableCollection<Player>();

            SelectedField = "Any"; // Default to "Any"
            SortOrder = "None"; // Default to "None"

            SearchCommand = new Command(async () => await SearchPlayers());
            NavigateToPlayerProfileCommand = new Command<Player>(async (player) => await NavigateToPlayerProfile(player));
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

        private async Task SearchPlayers()
        {
            if (string.IsNullOrEmpty(SelectedField) || SelectedField == "Any")
            {
                // If no specific field is selected, filter by any field and the entered filter value
                if (!string.IsNullOrEmpty(FilterValue))
                {
                    Players = new ObservableCollection<Player>(await playerDB.SelectByFilter("Any", FilterValue));
                }
                else if (!string.IsNullOrEmpty(SortOrder) && SortOrder != "None")
                {
                    // If no filter value is provided but a sort order is specified, apply sorting
                    Players = new ObservableCollection<Player>(await playerDB.SelectAndSort("Any", SortOrder));
                }
                else
                {
                    // If no filter value and no sort order are provided, return all players
                    Players = new ObservableCollection<Player>(await playerDB.SelectAllPlayers());
                }
            }
            else
            {
                // If a specific field is selected and a filter value is provided
                if (!string.IsNullOrEmpty(FilterValue))
                {
                    // Apply the filter based on the selected field and value
                    Players = new ObservableCollection<Player>(await playerDB.SelectByFilter(SelectedField, FilterValue));
                }
                else if (!string.IsNullOrEmpty(SortOrder) && SortOrder != "None")
                {
                    // If no filter value is provided but a sort order is specified, apply sorting
                    Players = new ObservableCollection<Player>(await playerDB.SelectAndSort(SelectedField, SortOrder));
                }
                else
                {
                    // If no filter value and no sort order are provided, select all players for the selected field
                    Players = new ObservableCollection<Player>(await playerDB.SelectAllPlayers());
                }
            }
        }

        private async Task NavigateToPlayerProfile(Player player)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("player", player);
            await Shell.Current.GoToAsync("/rPlayerProfile", data);
        }
    }
}