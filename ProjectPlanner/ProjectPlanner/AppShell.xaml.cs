using ProjectPlanner.ViewModels;
using ProjectPlanner.Views;

namespace ProjectPlanner
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("mainPage", typeof(MainPage));
            Routing.RegisterRoute("projectForm", typeof(ProjectForm));
            Routing.RegisterRoute("dailyDisplay", typeof(DailyDisplay));
            Routing.RegisterRoute("dailyTaskForm", typeof(DailyTaskForm));
            Routing.RegisterRoute("sprintView", typeof(SprintView));
            Routing.RegisterRoute("sprintForm", typeof(SprintForm));
            Routing.RegisterRoute("sprintPage", typeof(SprintPage));
            Routing.RegisterRoute("sprintTaskForm", typeof(SprintTaskForm));
        }
        private async void OnNavigateHomeClicked(object sender, EventArgs e)
        {
      
            await GoToAsync("mainPage");
        }

        private void OnSearchQueryChanged(object sender, EventArgs e)
        {
            if (sender is SearchHandler searchHandler)
            {
                // Implement what happens when the search query changes
                // You can call the search method from the ViewModel
                (BindingContext as MainPageViewModel)?.SearchCommand.Execute(searchHandler.Query);
            }
        }
    }
}
