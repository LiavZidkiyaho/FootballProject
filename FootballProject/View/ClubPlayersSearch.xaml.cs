using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class ClubPlayersSearch : ContentPage
{
	public ClubPlayersSearch(ClubPlayersSearchViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}