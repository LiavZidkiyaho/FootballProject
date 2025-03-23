using FootballProject.Model;
using FootballProject.Services;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    public class HomePageViewModel
    {
        private readonly UserService _userService;

        public User User { get; set; }

        // Command for Log Out
        public ICommand LogoutCommand { get; }

        public HomePageViewModel(UserService userService)
        {
            _userService = userService;

            // Initialize User from the service (assuming it's the logged-in user)
            User = _userService.GetCurrentUser();

            // Command to log out
            LogoutCommand = new Command(OnLogout);
        }

        // Method to handle log out action
        private async void OnLogout()
        {
            // Perform logout logic
            //_userService.Logout();
            await Shell.Current.GoToAsync("//rLogIn");
        }
    }
}
