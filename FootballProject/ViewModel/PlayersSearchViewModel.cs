using FootballProject.Model;
using FootballProject.ViewModel.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

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
            SearchCommand = new Command(async () => await SearchPlayers());
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

        private async Task SearchPlayers()
        {
            if (string.IsNullOrEmpty(FilterValue))
            {
                // If no filter value is provided, select all players
                Players = new ObservableCollection<Player>(await playerDB.SelectAllPlayers());
            }
            else if (string.IsNullOrEmpty(SelectedField) || SelectedField == "Any")
            {
                // If no specific field is selected, filter by any field
                Players = new ObservableCollection<Player>(await playerDB.SelectByFilter("Any", FilterValue));
            }
            else if (!string.IsNullOrEmpty(SortOrder) && SortOrder != "None")
            {
                // If a sort order is specified, sort the results
                Players = new ObservableCollection<Player>(await playerDB.SelectAndSort(SelectedField, SortOrder));
            }
            else
            {
                // Otherwise, filter by the selected field and value
                Players = new ObservableCollection<Player>(await playerDB.SelectByFilter(SelectedField, FilterValue));
            }
        }
    }
}