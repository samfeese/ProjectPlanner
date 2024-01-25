using ProjectPlanner.Views;

namespace ProjectPlanner
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("projectForm", typeof(ProjectForm));
        }
    }
}
