using FootballProject.View;

namespace FootballProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            ManagerTab.IsVisible = App.manager;
            if(App.role == "Coach")
            {
                ClubSearch.IsVisible = true;
            }
            else
            {
                ClubSearch.IsVisible = false;

            }
            Routing.RegisterRoute("rHomePage", typeof(HomePage));
            Routing.RegisterRoute("rSignUp", typeof(SignUp));
            Routing.RegisterRoute("rLogIn", typeof(LogIn));
            Routing.RegisterRoute("rPlayerProfile", typeof(PlayerProfile));
        }
    }
}
