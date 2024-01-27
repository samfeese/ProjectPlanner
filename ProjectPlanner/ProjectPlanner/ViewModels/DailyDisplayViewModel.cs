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
        private Project _project { get; set; }
        private DateTime displayDate = DateTime.Now;
        public ICommand ChangeDate { get; set; }

        public DailyDisplayViewModel() {

            AllDailyTasks = new ObservableCollection<DailyTask>();
            DailyTasksByDate = new ObservableCollection<DailyTask>();
            db = new DatabaseHelper();

            ChangeDate = new Command(ChangeDisplayDate);


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

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectId(_projectId);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentProject = p;
                Console.WriteLine(p.Name);
                Console.WriteLine(CurrentProject.Name);
                GetTasks.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasks.Add(dt);
                    OnPropertyChanged(nameof(GetTasks));
                }

            }
            );
        }

        private async void ChangeDisplayDate()
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
