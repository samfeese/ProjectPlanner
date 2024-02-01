using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class SprintForm : ContentPage
{
	public SprintForm()
	{
		InitializeComponent();
        BindingContext = new SprintFormViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SprintFormViewModel viewModel && viewModel.SprintId > 0)
        {
            viewModel.RetrieveSprint();
        }
    }
}