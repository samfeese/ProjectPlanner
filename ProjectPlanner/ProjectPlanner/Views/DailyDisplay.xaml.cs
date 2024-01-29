using ProjectPlanner.Models;
using ProjectPlanner.ViewModels;

namespace ProjectPlanner.Views;

public partial class DailyDisplay : ContentPage
{
    public DailyDisplay()
    {
        InitializeComponent();
        var viewModel = new DailyDisplayViewModel();
        BindingContext = viewModel;

       

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DailyDisplayViewModel viewModel)
        {

            await viewModel.Load();
        }


    }
    private void OnNotesEditorUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is DailyDisplayViewModel viewModel)
        {
            viewModel.SaveNoteCommand.Execute(null);
        }
    }
}