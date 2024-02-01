using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class SprintTaskForm : ContentPage
{
	public SprintTaskForm()
	{
		InitializeComponent();
        BindingContext = new SprintTaskFormViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SprintTaskFormViewModel viewModel && viewModel._taskId > 0)
        {
            viewModel.RetrieveTask();
        }
    }
}