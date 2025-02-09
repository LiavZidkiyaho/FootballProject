using FootballProject.View;

namespace FootballProject
{
    public partial class App : Application
    {
        public App(LogIn page)
        {
            InitializeComponent();
            MainPage = page;
        }


    }
}