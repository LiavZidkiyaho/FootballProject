using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballProject.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUser userService;

        private string username;
        private string password;

        public LoginViewModel(IUser service)
        {
            userService = service;
            LoginCommand = new Command(Login);
        }

        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
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
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand LoginCommand { get; }

        private async void Login()
        {
            var user = userService.GetUserByUsernameAndPassword(Username, Password);
            if (user != null)
            {
                await Shell.Current.GoToAsync("///MainPage");
            }
        }
    }
}

