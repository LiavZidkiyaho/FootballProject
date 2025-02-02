using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class LogIn : ContentPage
{
    public LogIn(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}   