using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class SprintView : ContentPage
{
	public SprintView()
	{
		InitializeComponent();
        BindingContext = new SprintViewViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SprintViewViewModel viewModel && viewModel._projectId > 0)
        {
            viewModel.Load();
        }
    }
}