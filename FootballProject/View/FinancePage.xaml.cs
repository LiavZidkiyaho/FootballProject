using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class FinancePage : ContentPage
{
	public FinancePage(FinancePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (App.role == "Manager")
        {
            AmountLabel.IsVisible = true;
            AmountEnrty.IsVisible = true;
            PurposeEnrty.IsVisible = true;
            PurposeLabel.IsVisible = true;
            Save.IsVisible = true;
        }
        else
        {
            AmountLabel.IsVisible = false;
            AmountEnrty.IsVisible = false;
            PurposeEnrty.IsVisible = false;
            PurposeLabel.IsVisible = false;
            Save.IsVisible = false;
        }
    }
}