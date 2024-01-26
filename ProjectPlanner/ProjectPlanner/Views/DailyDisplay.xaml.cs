using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class DailyDisplay : ContentPage
{
	public DailyDisplay()
	{
		InitializeComponent();
        BindingContext = new DailyDisplayViewModel();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DailyDisplayViewModel viewModel)
        {

            await viewModel.Load();
        }


    }
}