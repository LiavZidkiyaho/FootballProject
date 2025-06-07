using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private readonly IUser service; // Now works with WebService or UserService
        private User user;

        public ICommand NavToRegisterCommand { get; }
        public ICommand LoginUserCommand { get; }

        public LoginViewModel(IUser service)
        {
            this.service = service;
            LoginUserCommand = new Command(async () => await LoginUser());
            NavToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("RegisterPage"));

            Username = "Bruh";   // default for testing
            Password = "Bruh";   // default for testing
        }

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

        public bool CanLogin => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

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
                var user = await service.GetUser(Username);
                service.SetCurrentUser(user);
                if(user.IsAdmin == "True")
                {
                    App.manager = true;
                }
                else
                {
                    App.manager = false;
                }
                App.role = user.Role;
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
