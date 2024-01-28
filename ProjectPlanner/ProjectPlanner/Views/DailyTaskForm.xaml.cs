using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class DailyTaskForm : ContentPage
{
    public DailyTaskForm()
    {
        InitializeComponent();
        BindingContext = new DailyTaskFormViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DailyTaskFormViewModel viewModel && viewModel._taskId > 0)
        {
            viewModel.RetrieveTask();
        }
    }
}