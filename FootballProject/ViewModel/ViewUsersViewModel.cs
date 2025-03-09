using FootballProject.Model;
using FootballProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FootballProject.ViewModel
{
    public class ViewUsersViewModel : ViewModelBase
    {
        private bool isRefreshing;
        private bool isAdmin;
        UserService userService;

        public List<User> users;
        private User? user;

        public ObservableCollection<User> observableUsers;

        public ObservableCollection<User> ObservableUsers
        {
            get { return observableUsers; }
            set
            {
                if (observableUsers != value)
                {
                    observableUsers = value;
                    OnPropertyChanged(nameof(ObservableUsers));
                }
            }
        }

        public List<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                if (value != null)
                {
                    users = value;
                    ObservableUsers = new ObservableCollection<User>(users);
                    OnPropertyChanged(nameof(Users));
                    Refresh();
                }
            }
        }

        public ViewUsersViewModel(UserService service)
        {
            userService = service;
            users = userService.GetAllUsers(); //original List
            observableUsers = new ObservableCollection<User>();
            foreach (User user in users)
            {
                observableUsers.Add(user);
            }
            RefreshCommand = new Command(Refresh);
            AddCommand = new Command(addCommand);
            DeleteCommand = new Command<User>((u) => { if (userService.DeleteUser(u)) Refresh(); });
            EditCommand = new Command<User>(async (user) => { await EditMethodeCommand(user); });
        }
      

        public ICommand RefreshCommand
        { get; private set; }

        public ICommand EditCommand
        { get; private set; }

        public ICommand DeleteCommand
        { get; private set; }

        public ICommand AddCommand { get; private set; }


        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value) { isRefreshing = value; OnPropertyChanged(); }
            }
        }
        public async void Refresh()
        {
            IsRefreshing = true;
            users = await userService.GetAllUsers();
            ObservableUsers = null;
            ObservableUsers = new ObservableCollection<User>(users);
            IsRefreshing = false;
        }

        public async void addCommand()
        {
            await Shell.Current.GoToAsync("/rSignUp");
        }

        private async Task EditMethodeCommand(User user)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("user", user);
            await Shell.Current.GoToAsync("rSignUp", data);
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                if (isAdmin != value)
                {
                    isAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                }
            }
        }

        public User GiveCurrentUser()
        {
            return userService.GetCurrentUser();
        }
    }
}
