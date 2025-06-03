using FootballProject.Model;
using FootballProject.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    public class ViewUsersViewModel : ViewModelBase
    {
        private readonly IUser userService;
        private bool isRefreshing;
        private bool isAdmin;
        private ObservableCollection<User> observableUsers;

        public ObservableCollection<User> ObservableUsers
        {
            get => observableUsers;
            set
            {
                observableUsers = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        public ViewUsersViewModel(IUser service)
        {
            userService = service;

            ObservableUsers = new ObservableCollection<User>();

            RefreshCommand = new Command(async () => await RefreshAsync());
            AddCommand = new Command(async () => await GoToAddUserPage());
            DeleteCommand = new Command<User>(async (u) => await DeleteUserAsync(u));
            EditCommand = new Command<User>(async (u) => await EditUserAsync(u));

            _ = LoadUsersAsync(); // fire and forget
        }

        public async Task LoadUsersAsync()
        {
            var users = await userService.GetAllUsers();
            ObservableUsers = new ObservableCollection<User>(users);
        }

        public async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadUsersAsync();
            IsRefreshing = false;
        }

        private async Task DeleteUserAsync(User user)
        {
            if (user == null) return;

            bool deleted = await userService.DeleteUser(user);
            if (deleted)
            {
                await RefreshAsync();
            }
        }

        private async Task EditUserAsync(User user)
        {
            if (user == null) return;

            var data = new Dictionary<string, object>
            {
                { "user", user }
            };

            await Shell.Current.GoToAsync("rSignUp", data);
        }

        private async Task GoToAddUserPage()
        {
            await Shell.Current.GoToAsync("rSignUp");
        }
    }
}
