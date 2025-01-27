using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class SignUp : ContentPage
{
	public SignUp(SignUpViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}