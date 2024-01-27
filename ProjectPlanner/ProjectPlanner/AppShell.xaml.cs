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
        }
    }
}
