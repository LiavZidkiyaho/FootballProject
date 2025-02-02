namespace FootballProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void LogIn(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///rLogIn");
        }
    }

}
