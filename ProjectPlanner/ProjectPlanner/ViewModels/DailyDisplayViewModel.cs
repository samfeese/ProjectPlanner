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
        private ObservableCollection<DailyTask> DailyTasksByDate { get; set; }
        private Project _project { get; set; }
        private DateTime displayDate = DateTime.Now;
        public ICommand IncrementDate { get; set; }
        public ICommand DecrementDate { get; set; }

        public DailyDisplayViewModel() {
            DailyTasksByDate = new ObservableCollection<DailyTask>();
            db = new DatabaseHelper();

            IncrementDate = new Command(IncrementDisplayDate);
            DecrementDate = new Command(DecrementDisplayDate);


        }

        public Project CurrentProject
        {
            get => _project;
            set 
            { 
                _project = value;
                OnPropertyChanged(nameof(CurrentProject));
            }
        }

        public DateTime DisplayDate 
        {
            get => displayDate;
            set  
            {
                displayDate = value;
                OnPropertyChanged(nameof(DisplayDate));
            }
        }

        public ObservableCollection<DailyTask> GetTasksByDate
        {
            get => DailyTasksByDate;
            set
            {
                DailyTasksByDate = value;
                OnPropertyChanged(nameof(GetTasksByDate));
            }
        }

        public int ProjectID
        {
            get => _projectId;
            set
            {
                _projectId = value;
                OnPropertyChanged(nameof(ProjectID));
            }
        }

        public async Task Load()
        {

            Project p = await db.GetSingleAsync<Project>(_projectId);

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(_projectId, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentProject = p;
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            }
            );
        }

        private async void IncrementDisplayDate()
        {
            DisplayDate = DisplayDate.AddDays(1);

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            }
            );
        }

        private async void DecrementDisplayDate()
        {
            DisplayDate = DisplayDate.AddDays(-1);

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
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
