using ProjectPlanner.Models;
using ProjectPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlanner
{
    public class CustomSearchHandler : SearchHandler
    {

        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                (BindingContext as MainPageViewModel)?.SearchCommandDefault.Execute(newValue);
            }
            (BindingContext as MainPageViewModel)?.SearchCommand.Execute(newValue);
            
        }

    }
}
