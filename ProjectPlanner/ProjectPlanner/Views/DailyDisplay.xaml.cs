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
    private void OnNotesSaved(object sender, EventArgs e)
    {
        if (BindingContext is DailyDisplayViewModel viewModel)
        {
            viewModel.SaveNoteCommand.Execute(null);
            NotesEditor.Unfocus();
        }
    }
    private void OnLayoutTap(object sender, EventArgs e)
    {
        if (NotesEditor.IsFocused)
        {
            NotesEditor.Unfocus();
            NotesEditor.IsEnabled = false;
            NotesEditor.IsEnabled = true;
            
        }
    }
}