using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class PlayersSearch : ContentPage
{
	public PlayersSearch(PlayersSearchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}