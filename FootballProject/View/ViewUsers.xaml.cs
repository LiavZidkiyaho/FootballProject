using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class ViewUsers : ContentPage
{
	public ViewUsers(ViewUsersViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}