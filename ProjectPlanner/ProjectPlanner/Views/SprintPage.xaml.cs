using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class SprintPage : ContentPage
{
	public SprintPage()
	{
		InitializeComponent();
		BindingContext = new SprintPageViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SprintPageViewModel viewModel)
        {
            viewModel.RetrieveSprintTasks();
        }
    }
}