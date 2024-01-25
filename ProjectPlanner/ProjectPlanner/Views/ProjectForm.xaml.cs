using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;


public partial class ProjectForm : ContentPage
{

    public ProjectForm()
	{
        InitializeComponent();
      
		BindingContext = new ProjectFormViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProjectFormViewModel viewModel && viewModel._projectId > 0)
        {
            viewModel.RetrieveProject();
        }
    }
}