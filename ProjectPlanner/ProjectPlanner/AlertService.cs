using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPlanner.Interfaces;

namespace ProjectPlanner
{
    public class AlertService : IAlertService
    {
        public async Task ShowAlert(string title, string message, string btn)
        {
            await Shell.Current.DisplayAlert(title, message, btn);
        }
    }
}
