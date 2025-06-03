using FootballProject.Model;
using FootballProject.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    public class PlayersSearchViewModel : ViewModelBase
    {
        private readonly IUser service;

        private ObservableCollection<Player> players;
        private string selectedField;
        private string filterValue;
        private string sortOrder;

        public PlayersSearchViewModel(IUser webService)
        {
            service = webService;

            players = new ObservableCollection<Player>();
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
