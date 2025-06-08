using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    /// <summary>
    /// ViewModel responsible for handling user login logic and navigation.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private readonly IUser service;
        private User user;

        /// <summary>
        /// Command to navigate to the registration page.
        /// </summary>
        public ICommand NavToRegisterCommand { get; }

        /// <summary>
        /// Command to execute the login process.
        /// </summary>
        public ICommand LoginUserCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="service">User service used for retrieving and storing users.</param>
        public LoginViewModel(IUser service)
        {
            this.service = service;
            LoginUserCommand = new Command(async () => await LoginUser());
            NavToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("RegisterPage"));

            // Default credentials for testing (can be removed in production)
            Username = "Bruh";
            Password = "Bruh";
        }

        /// <summary>
        /// The username entered by the user.
        /// </summary>
        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        /// <summary>
        /// The password entered by the user.
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        /// <summary>
        /// The currently logged-in user.
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
        /// Indicates whether the login form is valid and can be submitted.
        /// </summary>
        public bool CanLogin => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        /// <summary>
        /// Attempts to log in the user using the entered credentials.
        /// </summary>
        private async Task LoginUser()
        {
            try
            {
                var fetchedUser = await service.GetUser(Username);

                if (fetchedUser == null || fetchedUser.Password != Password)
                {
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid username or password.", "OK");
                    return;
                }

                User = fetchedUser;
                await Application.Current.MainPage.DisplayAlert("Login Success", $"Welcome, {User.Name}!", "OK");

                service.SetCurrentUser(User);

                // Set global user flags
                App.manager = User.IsAdmin == "True";
                App.role = User.Role;

                // Replace the main page and navigate to the homepage
                App.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync("///rHomePage", new Dictionary<string, object>
                {
                    { "User", User }
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }
    }
}
