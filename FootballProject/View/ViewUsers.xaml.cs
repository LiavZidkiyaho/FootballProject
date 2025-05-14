using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class ViewUsers : ContentPage
{
    private ViewUsersViewModel viewModel;
	public ViewUsers(ViewUsersViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.RefreshAsync();
    }
}