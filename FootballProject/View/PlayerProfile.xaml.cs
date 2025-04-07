using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class PlayerProfile : ContentPage
{
	public PlayerProfile(PlayerProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}