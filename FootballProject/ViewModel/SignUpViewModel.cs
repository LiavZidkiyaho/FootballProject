using FootballProject.Model;
using FootballProject.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FootballProject.ViewModel
{
    /// <summary>
    /// ViewModel for registering new users or editing existing ones.
    /// Manages form validation, submission, and dynamic team/role selections.
    /// </summary>
    [QueryProperty(nameof(EditUser), "user")]
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IUser userService;
        private List<User> users = new();
        private User user = new();
        private User? editUser;
        private string name, username, password, email, errorMessage;
        private Team selectedTeam;
        private string selectedRole;
        private bool isAdmin;

        /// <summary>
        /// Constructor initializing commands and collections.
        /// </summary>
        public SignUpViewModel(IUser service)
        {
            userService = service;
            AddUserCommand = new Command(async () => await AddOrUpdateUser(), () => CanAddUser);
            AddTeamCommand = new Command(async () => await AddTeam());

            Roles = new ObservableCollection<string> { "Coach", "Manager" };
            Teams = new ObservableCollection<Team>();
            LoadTeams();
        }

        public ObservableCollection<Team> Teams { get; }
        public ObservableCollection<string> Roles { get; }

        public ICommand AddUserCommand { get; }
        public ICommand AddTeamCommand { get; }

        public Team SelectedTeam
        {
            get => selectedTeam;
            set { selectedTeam = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanAddUser)); }
        }

        public string SelectedRole
        {
            get => selectedRole;
            set { selectedRole = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanAddUser)); }
        }

        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotAdmin));
            }
        }

        public bool IsNotAdmin
        {
            get => !isAdmin;
            set
            {
                isAdmin = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAdmin));
            }
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

        public string ErrorMessage
        {
            get => errorMessage;
            set { errorMessage = value; OnPropertyChanged(); }
        }

        public bool HasError =>
            !ValidName() || !ValidUsername() || !ValidPassword() || !ValidEmail() ||
            SelectedTeam == null || string.IsNullOrWhiteSpace(SelectedRole);

        public bool CanAddUser => !HasError;

        /// <summary>
        /// If editing an existing user, this property pre-fills the form with current values.
        /// </summary>
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
                        IsAdmin = editUser.IsAdmin,
                        Role = editUser.Role
                    };

                    Name = editUser.Name;
                    Username = editUser.Username;
                    Password = editUser.Password;
                    Email = editUser.Email;
                    SelectedTeam = editUser.Team;
                    IsAdmin = editUser.IsAdmin == "True";
                    SelectedRole = editUser.Role;
                }
                else
                {
                    user = new User();
                    Name = Username = Password = Email = null;
                    SelectedTeam = null;
                    IsAdmin = false;
                    SelectedRole = null;
                }

                HandleError();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Loads available teams into the UI dropdown.
        /// </summary>
        private async void LoadTeams()
        {
            var list = await userService.GetAllTeams();
            Teams.Clear();
            foreach (var t in list)
                Teams.Add(t);
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

        /// <summary>
        /// Recalculates the form's error state and builds the error message.
        /// </summary>
        private void HandleError()
        {
            var errors = new List<string>();
            if (!ValidName()) errors.Add("Name is too short.");
            if (!ValidUsername()) errors.Add("Username is too short.");
            if (!ValidPassword()) errors.Add("Password must be at least 6 characters.");
            if (!ValidEmail()) errors.Add("Invalid email address.");
            if (SelectedTeam == null) errors.Add("Team must be selected.");
            if (string.IsNullOrWhiteSpace(SelectedRole)) errors.Add("Role must be selected.");
            ErrorMessage = string.Join("\n", errors);
            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(CanAddUser));
        }

        /// <summary>
        /// Adds a new user or updates an existing one.
        /// </summary>
        private async Task AddOrUpdateUser()
        {
            users = await userService.GetAllUsers();
            HandleError();
            if (HasError) return;

            if (EditUser != null && user != null && EditUser.Id == user.Id)
            {
                var existing = users.Find(u => u.Id == user.Id);
                if (existing != null)
                {
                    existing.Name = Name;
                    existing.Username = Username;
                    existing.Password = Password;
                    existing.Email = Email;
                    existing.Team = SelectedTeam;
                    existing.IsAdmin = IsAdmin ? "True" : "False";
                    existing.Role = SelectedRole;

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
                if (users.Exists(u => u.Username.Equals(Username, System.StringComparison.OrdinalIgnoreCase)))
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
                    Team = SelectedTeam,
                    IsAdmin = IsAdmin ? "True" : "False",
                    Role = SelectedRole
                };

                bool added = await userService.AddUser(user);
                if (added)
                {
                    await Shell.Current.DisplayAlert("Success", "User added successfully.", "OK");
                    await Shell.Current.GoToAsync("///rViewUsers");
                    EditUser = null;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to add user.", "Cancel");
                }
            }
        }

        /// <summary>
        /// Adds a new team using a prompt dialog.
        /// </summary>
        private async Task AddTeam()
        {
            string result = await Shell.Current.DisplayPromptAsync("New Team", "Enter team name:", "Add", "Cancel", "Team name");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var newTeam = new Team { team1 = result };
                var addedTeam = await userService.AddTeam(newTeam);

                if (addedTeam != null)
                {
                    var updatedTeams = await userService.GetAllTeams();
                    Teams.Clear();
                    foreach (var team in updatedTeams)
                        Teams.Add(team);

                    SelectedTeam = addedTeam;

                    await Shell.Current.DisplayAlert("Success", "Team added.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to add team.", "Cancel");
                }
            }
        }
    }
}
