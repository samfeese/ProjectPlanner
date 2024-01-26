using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlanner.ViewModels
{

    class DailyDisplayViewModel
    {

        readonly DatabaseHelper db;
        private ObservableCollection<DailyTask> AllDailyTasks { get; set; } // Provides the INotifyCollectionChanged interface, allowing UI update. I originally was going to just use a List<Terms> but MAUI as this collection
        public DailyDisplayViewModel() {
            AllDailyTasks = new ObservableCollection<DailyTask>();
            db = new DatabaseHelper();


        }

        public ObservableCollection<DailyTask> GetTasks
        {
            get => AllDailyTasks;
            set
            {
                AllDailyTasks = value;
                OnPropertyChanged(nameof(GetTasks));
            }
        }

        public async Task Load()
        {
            List<DailyTask> dbDailyTasks = await db.GetAllAsync<DailyTask>();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetTasks.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasks.Add(dt);
                    OnPropertyChanged(nameof(GetTasks));
                }

            }
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
