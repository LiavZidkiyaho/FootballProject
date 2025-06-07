using FootballProject.View;

namespace FootballProject
{
    public partial class App : Application
    {
        public static bool manager = false;
        public static string role = "";
        public App(LogIn page)
        {
            InitializeComponent();
            MainPage = page;
        }


    }
}