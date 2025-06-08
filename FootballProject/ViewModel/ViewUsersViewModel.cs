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
    /// ViewModel for managing and displaying the list of users.
    /// Supports loading, refreshing, editing, deleting, and navigating to user creation.
    /// </summary>
    public class ViewUsersViewModel : ViewModelBase
    {
        private readonly IUser userService;
        private bool isRefreshing;
        private bool isAdmin;
        private ObservableCollection<User> observableUsers;

        /// <summary>
        /// The list of users displayed in the UI.
        /// </summary>
        public ObservableCollection<User> ObservableUsers
        {
            get => observableUsers;
            set
            {
                observableUsers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the user list is currently refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates if the current user is an admin (can control visibility of certain UI features).
        /// </summary>
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand RefreshCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        /// <summary>
        /// Constructor initializes the user service and binds all commands.
        /// </summary>
        /// <param name="service">The service that handles user data.</param>
        public ViewUsersViewModel(IUser service)
        {
            userService = service;

            ObservableUsers = new ObservableCollection<User>();

            RefreshCommand = new Command(async () => await RefreshAsync());
            AddCommand = new Command(async () => await GoToAddUserPage());
            DeleteCommand = new Command<User>(async (u) => await DeleteUserAsync(u));
            EditCommand = new Command<User>(async (u) => await EditUserAsync(u));

            _ = LoadUsersAsync(); // Load users on initialization (fire-and-forget)
        }

        /// <summary>
        /// Loads the user list from the service.
        /// </summary>
        public async Task LoadUsersAsync()
        {
            var users = await userService.GetAllUsers();
            ObservableUsers = new ObservableCollection<User>(users);
        }

        /// <summary>
        /// Refreshes the user list with loading indication.
        /// </summary>
        public async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadUsersAsync();
            IsRefreshing = false;
        }

        /// <summary>
        /// Deletes the specified user from the database.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        private async Task DeleteUserAsync(User user)
        {
            if (user == null) return;

            bool deleted = await userService.DeleteUser(user);
            if (deleted)
            {
                await RefreshAsync();
            }
        }

        /// <summary>
        /// Navigates to the sign-up page for editing an existing user.
        /// </summary>
        /// <param name="user">The user to edit.</param>
        private async Task EditUserAsync(User user)
        {
            if (user == null) return;

            var data = new Dictionary<string, object>
            {
                { "user", user }
            };

            await Shell.Current.GoToAsync("rSignUp", data);
        }

        /// <summary>
        /// Navigates to the sign-up page for adding a new user.
        /// </summary>
        private async Task GoToAddUserPage()
        {
            await Shell.Current.GoToAsync("rSignUp");
        }
    }
}
