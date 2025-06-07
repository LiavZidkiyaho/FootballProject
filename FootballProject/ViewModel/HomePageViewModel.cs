using FootballProject.Model;
using FootballProject.Services;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(User), "User")]
    public class HomePageViewModel : ViewModelBase
    {
        
        private readonly IUser _userService;
        private User user;

        public User User
        {
            get => user;
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LogoutCommand { get; }

        public HomePageViewModel(IUser userService)
        {
            _userService = userService;
            LogoutCommand = new Command(async () => await OnLogoutAsync());
        }

        private async Task OnLogoutAsync()
        {
            // Optional: clear current user if applicable
            if (_userService is IUser us)
            {
                us.SetCurrentUser(null);
            }

            // Navigate to login page
            await Shell.Current.GoToAsync("/rLogIn");
        }
    }
}
