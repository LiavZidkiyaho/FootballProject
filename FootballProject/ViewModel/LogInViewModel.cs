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
        private string username, password;
        UserService service;
        List<Model.User> users;
        private Model.User user;


        public ICommand NavToRegisterCommand { get; }
        public ICommand LoginUserCommand { get; }
        public LoginViewModel(UserService service)
        {
            this.service = service;
            LoginUserCommand = new Command(LoginUser);
            service.init();
            Username = "Bruh";
            Password = "Bruh";
        }

        public Model.User User
        {
            get => user;
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }



        public bool IsValidUser()
        {
            if (service.GetUser(Username) != null) return true;
            return false;
        }

        public bool CanLogin
        {
            get
            {
                return Username != null && Password != null;
            }
        }

        public async void LoginUser()
        {
            
            if (IsValidUser())
            {
                User = await service.GetUser(Username);
                service.CurrentUser = User;
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("User", User);
                if (user == null || (Username != User.Username) || (Password != User.Password))
                    await Application.Current.MainPage.DisplayAlert("Login", "Login Faild!", "ok");
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login", $"Login Succeed! for {user.Name}", "ok");
                    App.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("///rHomePage", data);
                }
            }
        }

    }
}

