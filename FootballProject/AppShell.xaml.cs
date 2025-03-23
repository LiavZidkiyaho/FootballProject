using FootballProject.View;

namespace FootballProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("rHomePage", typeof(HomePage));
            Routing.RegisterRoute("rSignUp", typeof(SignUp));
            Routing.RegisterRoute("rLogIn", typeof(LogIn));
        }
    }
}
