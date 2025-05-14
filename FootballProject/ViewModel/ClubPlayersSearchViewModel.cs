using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FootballProject.ViewModel
{
    public class ClubPlayersSearchViewModel : ViewModelBase
    {
        private readonly PlayerDB playerDB;
        private readonly UserService userService;
        private ObservableCollection<Player> players;
        private string selectedField;
        private string filterValue;
        private string sortOrder;

        public ClubPlayersSearchViewModel(UserService service)
        {
            playerDB = new PlayerDB();
            players = new ObservableCollection<Player>();
            userService = service;

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
            var manager = userService.CurrentUser;
            var teamId = manager.Team.Id;

            if (!string.IsNullOrEmpty(FilterValue))
            {
                Players = new ObservableCollection<Player>(
                    await playerDB.SelectTeamPlayersByFirstName(teamId, FilterValue));
            }
            else
            {
                Players = new ObservableCollection<Player>(
                    await playerDB.SelectPlayersByTeam(teamId));
            }
        }

        private async Task NavigateToPlayerProfile(Player player)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("player", player);
            data.Add("source", "ClubPlayersSearch");
            await Shell.Current.GoToAsync("/rPlayerProfile", data);
        }
    }
}
