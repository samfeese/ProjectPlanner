using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(ProjectID), "projectId")]
    class DailyDisplayViewModel
    {
        public int _projectId = -1;
        readonly DatabaseHelper db;
        private ObservableCollection<DailyTask> AllDailyTasks { get; set; }
        private ObservableCollection<DailyTask> DailyTasksByDate { get; set; }
        private DateTime displayDate = DateTime.Now;
        public ICommand ChangeDate { get; set; }

        public DailyDisplayViewModel() {

            AllDailyTasks = new ObservableCollection<DailyTask>();
            DailyTasksByDate = new ObservableCollection<DailyTask>();
            db = new DatabaseHelper();

            ChangeDate = new Command(ChangeDisplayDate);


        }

        public DateTime DisplayDate 
        {
            get => displayDate;
            set => displayDate = value;
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

        public ObservableCollection<DailyTask> GetTasksByDate
        {
            get => DailyTasksByDate;
            set
            {
                DailyTasksByDate = value;
                OnPropertyChanged(nameof(GetTasks));
            }
        }

        public ObservableCollection<DailyTask> FilterByDate
        {
            get => DailyTasksByDate;
            set
            {

            }
        }

        public int ProjectID
        {
            set
            {
                _projectId = value;
            }
        }

        public async Task Load()
        {
            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectId(_projectId);
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

        private void ChangeDisplayDate()
        {
            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(_projectId, displayDate);
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
