using ProjectPlanner.Models;
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

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is DailyTask task)
        {
            var viewModel = (DailyDisplayViewModel)BindingContext;
            if (!viewModel.IsLoadingData)
            {
                viewModel.UpdateCompleteCommand(task);
            }
        }
    }
}