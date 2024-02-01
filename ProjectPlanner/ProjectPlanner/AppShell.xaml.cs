﻿using ProjectPlanner.Views;

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
        }
    }
}
