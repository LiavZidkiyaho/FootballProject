using FootballProject.ViewModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace FootballProject.View;

public partial class PlayerProfile : ContentPage
{
    private PlayerProfileViewModel ViewModel => BindingContext as PlayerProfileViewModel;

    public PlayerProfile(PlayerProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (ViewModel?.Player?.Position != null)
        {
            HighlightPosition(ViewModel.Player.Position);
        }
    }

    private void ResetEllipseColors()
    {
        gk.Fill = fb1.Fill = cb.Fill = fb2.Fill =
        dm.Fill = cm.Fill = winger1.Fill = am.Fill = winger2.Fill = st.Fill =
            new SolidColorBrush(Colors.LightGray);
    }


    private void HighlightPosition(string position)
    {
        ResetEllipseColors();

        switch (position.ToUpperInvariant())
        {
            case "GK": gk.Fill = new SolidColorBrush(Colors.Green); break;
            case "FB": fb1.Fill = fb2.Fill = new SolidColorBrush(Colors.Green); break;
            case "CB": cb.Fill = new SolidColorBrush(Colors.Green); break;
            case "DM": dm.Fill = new SolidColorBrush(Colors.Green); break;
            case "CM": cm.Fill = new SolidColorBrush(Colors.Green); break;
            case "AM": am.Fill = new SolidColorBrush(Colors.Green); break;
            case "WINGER": winger1.Fill = winger2.Fill = new SolidColorBrush(Colors.Green); break;
            case "ST": st.Fill = new SolidColorBrush(Colors.Green); break;
            default: break;
        }
    }

}
