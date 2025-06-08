using FootballProject.Model;
using FootballProject.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    /// <summary>
    /// ViewModel for the Home Page.
    /// Manages the current user and handles logout functionality.
    /// </summary>
    [QueryProperty(nameof(User), "User")]
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IUser _userService;
        private User user;

        /// <summary>
        /// Gets or sets the current user (injected from navigation).
        /// </summary>
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

        /// <summary>
        /// Command to handle user logout.
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// Constructor that injects the user service and initializes the logout command.
        /// </summary>
        /// <param name="userService">User service implementation (WebService or other).</param>
        public HomePageViewModel(IUser userService)
        {
            _userService = userService;
            LogoutCommand = new Command(async () => await OnLogoutAsync());
        }

        /// <summary>
        /// Clears the current user and navigates to the login screen.
        /// </summary>
        private async Task OnLogoutAsync()
        {
            _userService.SetCurrentUser(null);
            await Shell.Current.GoToAsync("/rLogIn");
        }
    }
}
