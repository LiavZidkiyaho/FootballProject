using FootballProject.ViewModel;
using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace FootballProject.ViewModel
{
    public class SignUpViewModel : ViewModelBase
    {
        private Model.User user = new Model.User();
        private List<Model.User> users = new List<Model.User>();
        private readonly UserService userService;

        private string name;
        private string username;
        private string password;
        private string email;
        private string errorMessage;

        public SignUpViewModel(UserService service)
        {
            this.userService = service;
            AddUserCommand = new Command(AddNewUser);
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

        private void AddNewUser()
        {
            user = new Model.User()
            {
                Id = users.Count + 1, // Auto-increment ID
                Name = this.Name,
                Username = this.Username,
                Password = this.Password,
                Email = this.Email
            };

            bool userExists = false;

            for (int i = 0; i < users.Count; i++)
            {
                if (user.Id == users[i].Id)
                {
                    users[i] = user;
                    userExists = true;
                    break;
                }
            }

            if (!userExists)
            {
                users.Add(user);
                userService.CurrentUser = user;
            }
            UpdateUser();
        }

        private async void UpdateUser()
        {
            await Shell.Current.GoToAsync("///rMainPage");
        }
    }
}