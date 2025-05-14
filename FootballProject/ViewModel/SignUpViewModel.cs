using FootballProject.Model;
using FootballProject.Services;
using FootballProject.ViewModel.DB;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    [QueryProperty(nameof(EditUser), "user")]
    public class SignUpViewModel : ViewModelBase
    {
        private readonly UserService userService;
        private List<User> users = new List<User>();
        private User user = new User();
        private User? editUser;
        private string name, username, password, email;
        private Team team;
        private string isAdmin = "No";
        private string errorMessage;

        public SignUpViewModel(UserService service)
        {
            userService = service;
            _ = userService.initUsers();
            AddUserCommand = new Command(async () => await AddOrUpdateUser());
        }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); HandleError(); }
        }

        public string Username
        {
            get => username;
            set { username = value; OnPropertyChanged(); HandleError(); }
        }

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); HandleError(); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); HandleError(); }
        }

        public Team Team
        {
            get => team;
            set { team = value; OnPropertyChanged(); }
        }

        public string IsAdmin
        {
            get => isAdmin;
            set { isAdmin = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set { errorMessage = value; OnPropertyChanged(); }
        }

        public bool HasError => !ValidName() || !ValidUsername() || !ValidPassword() || !ValidEmail();

        public bool CanAddUser => !HasError;

        public ICommand AddUserCommand { get; }

        public User? EditUser
        {
            get => editUser;
            set
            {
                editUser = value;
                if (editUser != null)
                {
                    user = new User
                    {
                        Id = editUser.Id,
                        Name = editUser.Name,
                        Username = editUser.Username,
                        Password = editUser.Password,
                        Email = editUser.Email,
                        Team = editUser.Team,
                        IsAdmin = editUser.IsAdmin
                    };

                    Name = editUser.Name;
                    Username = editUser.Username;
                    Password = editUser.Password;
                    Email = editUser.Email;
                    Team = editUser.Team;
                    IsAdmin = editUser.IsAdmin;
                }
                else
                {
                    user = new User();
                    Name = Username = Password = Email = null;
                    Team = null;
                    IsAdmin = "No";
                }
                HandleError();
                OnPropertyChanged();
            }
        }

        private bool ValidName() => !string.IsNullOrWhiteSpace(Name) && Name.Length >= 3;

        private bool ValidUsername() => !string.IsNullOrWhiteSpace(Username) && Username.Length >= 3;

        private bool ValidPassword() => !string.IsNullOrWhiteSpace(Password) && Password.Length >= 6;

        private bool ValidEmail()
        {
            if (string.IsNullOrWhiteSpace(Email)) return false;
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(Email, pattern);
        }

        private void HandleError()
        {
            var errors = new List<string>();
            if (!ValidName()) errors.Add("Name is too short.");
            if (!ValidUsername()) errors.Add("Username is too short.");
            if (!ValidPassword()) errors.Add("Password must be at least 6 characters.");
            if (!ValidEmail()) errors.Add("Invalid email address.");
            ErrorMessage = string.Join("\n", errors);
            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(CanAddUser));
        }

        private async Task AddOrUpdateUser()
        {
            users = await userService.GetAllUsers();
            HandleError();
            if (HasError) return;

            // Editing existing user
            if (EditUser != null && user != null && EditUser.Id == user.Id)
            {
                var existing = users.Find(u => u.Id == user.Id);
                if (existing != null)
                {
                    existing.Name = Name;
                    existing.Username = Username;
                    existing.Password = Password;
                    existing.Email = Email;
                    existing.Team = Team;
                    existing.IsAdmin = IsAdmin;

                    bool updated = await userService.UpdateUser(existing);
                    if (updated)
                    {
                        await Shell.Current.DisplayAlert("Success", "User updated successfully.", "OK");
                        await Shell.Current.GoToAsync("///rViewUsers");
                        EditUser = null;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Failed to update user.", "Cancel");
                    }
                }
            }
            else
            {
                // Check for duplicate username
                if (users.Exists(u => u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)))
                {
                    await Shell.Current.DisplayAlert("Error", "Username already exists.", "OK");
                    return;
                }

                user = new User
                {
                    Id = users.Count + 1,
                    Name = Name,
                    Username = Username,
                    Password = Password,
                    Email = Email,
                    Team = Team,
                    IsAdmin = IsAdmin
                };

                bool added = await userService.AddUser(user);
                if (added)
                {
                    await Shell.Current.DisplayAlert("Success", "User added successfully.", "OK");
                    await Shell.Current.GoToAsync("///rViewUsers");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to add user.", "Cancel");
                }
                EditUser = null;
            }
        }
    }
}
