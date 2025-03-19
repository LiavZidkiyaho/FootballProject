using FootballProject.ViewModel;
using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using FootballProject.ViewModel.DB;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(EditUser), "user")]
    public class SignUpViewModel : ViewModelBase
    {
        private Model.User user = new Model.User();
        private List<Model.User> users = new List<Model.User>();
        private readonly UserService userService;
        private UserDB db = new UserDB();
        private Model.User? editUser;

        private string name;
        private string username;
        private string password;
        private string email;
        private string team;
        private string isAdmin = "No";
        private string errorMessage;

        public SignUpViewModel(UserService service)
        {
            this.userService = service;
            service.init();
            AddUserCommand = new Command<string>(AddnewUser);
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                    HandleError();
                }
            }
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
                    HandleError();
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
                    HandleError();
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                    HandleError();
                }
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public string IsAdmin 
        { 
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
        public string Team 
        {
            get => team;
            set
            {
                team = value;
                OnPropertyChanged(nameof(Team));
            }
        }

        private bool ValidName() => !string.IsNullOrWhiteSpace(Name) && Name.Length >= 3;

        private bool ValidUsername() => !string.IsNullOrWhiteSpace(Username) && Username.Length >= 3;

        private bool ValidPassword() => !string.IsNullOrWhiteSpace(Password) && Password.Length >= 6;

        private bool ValidEmail()
        {
            if (string.IsNullOrEmpty(Email)) return false;
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(Email, emailPattern);
        }

        private void HandleError()
        {
            var errors = new List<string>();

            if (!ValidName()) errors.Add("Name is too short or contains invalid characters.");
            if (!ValidUsername()) errors.Add("Username is too short.");
            if (!ValidPassword()) errors.Add("Password must be at least 6 characters long.");
            if (!ValidEmail()) errors.Add("Invalid email address.");

            ErrorMessage = string.Join("\n", errors);

            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(CanAddUser));
        }

        public bool HasError => !ValidName() || !ValidUsername() || !ValidPassword() || !ValidEmail();

        public bool CanAddUser => !HasError;

        public ICommand AddUserCommand { get; private set; }

        public Model.User? EditUser
        {
            get { return editUser; }
            set
            {
                if (value == null) editUser = null;
                if (value != null && EditUser != value)
                {
                    editUser = value;
                    user = new Model.User();
                    user = value;
                    Name = user.Name;
                    Username = user.Username;
                    Password = user.Password;
                    Email = user.Email;
                    Team = user.Team;
                    isAdmin = user.IsAdmin;
                    OnPropertyChanged(nameof(EditUser));
                    HandleError();
                }
                else
                {
                    editUser = null;
                    user = new Model.User();
                    user = value;
                    Name = null;
                    Username = null;
                    Password = null;
                    Email = null;
                    Team = null;
                    OnPropertyChanged(nameof(EditUser));
                    HandleError();
                }
            }
        }

        private async void AddnewUser(string name)
        {
            users = await userService.GetAllUsers();
            if (EditUser != null && user != null && EditUser.Id == user.Id)
            {
                OnPropertyChanged(nameof(AddnewUser));
                HandleError();
                int i = 0;
                for (; i < users.Count; i++)
                {
                    if (EditUser.Id == users[i].Id)
                    {
                        users[i].Name = this.Name;
                        users[i].Username = this.Username;
                        users[i].Password = this.Password;
                        users[i].Email = this.Email;
                        users[i].Team = this.Team;
                        users[i].IsAdmin = this.IsAdmin;

                        break;
                    }
                }

                bool f = await userService.UpdateUser(users[i]);
                if (f)
                {
                    await Shell.Current.DisplayAlert(title: "Updated user or not", message: "user updated succsesfully", cancel: "Go back");
                    await Shell.Current.GoToAsync("///rViewUsers");
                    EditUser = null;

                }
                else Shell.Current.DisplayAlert(title: "Updated user or not", message: "Error, user was not updated", cancel: "Cancel");
                EditUser = null;

            }
            else
            {
                OnPropertyChanged(nameof(AddnewUser));
                HandleError();
                user = new Model.User()
                {
                    Id = users.Count + 1,
                    Name = this.Name,
                    Username = this.Username,
                    Password = this.Password,
                    Email = this.Email,
                    Team = this.Team,
                    IsAdmin = this.IsAdmin,
                };


                bool f = await userService.AddUser(user);
                if (f)
                {
                    await Shell.Current.DisplayAlert(title: "Added user or not", message: "Added user succsesfully", cancel: "Go back");
                    await Shell.Current.GoToAsync("///rViewUsers");

                }
                else Shell.Current.DisplayAlert(title: "Added user or not", message: "Error, user was not added", cancel: "Cancel");
                EditUser = null;


            }
        }

            private async void UpdateUser()
            {
                await Shell.Current.GoToAsync("///rViewUsers");
            }
    }
}