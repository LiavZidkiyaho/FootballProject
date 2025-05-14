using FootballProject.Model;
using FootballProject.ViewModel.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

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

            SelectedField = "Any"; // Default field
            SortOrder = "None";    // Default sort

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
            if (SelectedField == "Any" || string.IsNullOrEmpty(SelectedField))
            {
                if (!string.IsNullOrWhiteSpace(FilterValue))
                {
                    Players = new ObservableCollection<Player>(
                        await playerDB.SelectByFilter("Any", FilterValue));
                }
                else if (!string.IsNullOrWhiteSpace(SortOrder) && SortOrder != "None")
                {
                    Players = new ObservableCollection<Player>(
                        await playerDB.SelectAndSort("Any", SortOrder));
                }
                else
                {
                    Players = new ObservableCollection<Player>(
                        await playerDB.SelectAllPlayers());
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(FilterValue))
                {
                    Players = new ObservableCollection<Player>(
                        await playerDB.SelectByFilter(SelectedField, FilterValue));
                }
                else if (!string.IsNullOrWhiteSpace(SortOrder) && SortOrder != "None")
                {
                    Players = new ObservableCollection<Player>(
                        await playerDB.SelectAndSort(SelectedField, SortOrder));
                }
                else
                {
                    Players = new ObservableCollection<Player>(
                        await playerDB.SelectAllPlayers());
                }
            }
        }

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
