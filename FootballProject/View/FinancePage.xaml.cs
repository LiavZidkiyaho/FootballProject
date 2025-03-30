using FootballProject.ViewModel;

namespace FootballProject.View;

public partial class FinancePage : ContentPage
{
	public FinancePage(FinancePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}