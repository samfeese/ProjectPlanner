using ProjectPlanner.Models;
using System.ComponentModel;

using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(ProjectId), "projectId")]
    [QueryProperty(nameof(TaskId), "taskId")]
    [QueryProperty(nameof(TaskDateString), "taskDateString")]
    public class DailyTaskFormViewModel : INotifyPropertyChanged
    {
        public int _taskId = -1;
        public int _projectId = -1;
        public DateTime _taskDate;
        public string _taskDateString;
        readonly DatabaseHelper db;
        private DailyTask daily;

        private string taskName;
        private bool completed;

        public List<string> StatusOptions { get; set; }

        public ICommand SaveTask { get; set; }
        public ICommand DeleteTask { get; set; }


        public DailyTaskFormViewModel()
        {
            db = new DatabaseHelper();
            SaveTask = new Command(AddTask);
            DeleteTask = new Command(DeleteTaskBtn);
            StatusOptions = ["Complete", "Not Complete"];
            SelectedStatus = StatusOptions[1];
        }

        public int ProjectId
        {
            set
            {
                _projectId = value;

            }
        }
        public int TaskId
        {
           
            set
            {
                _taskId = value;
            }

        }
        private string selectedStatus;
        public string SelectedStatus
        {
            get => selectedStatus;
            set
            {
                selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }
        public DateTime TaskDate
        {
            get { return _taskDate; }
            set
            {
                _taskDate = value;
            }
        }
        public string TaskDateString
        {
            get { return _taskDateString; }
            set
            {
                _taskDateString = value;
            }
        }

        public string TaskName
        {
            get => taskName;
            set
            {
                taskName = value;
                OnPropertyChanged(nameof(TaskName));
            }
        }

        public bool Completed
        {
            get => completed;
            set
            {
                completed = value;
                OnPropertyChanged(nameof(Completed));
            }
        }

        public async void RetrieveTask()
        {
            DailyTask dt = await db.GetSingleAsync<DailyTask>(_taskId);


            if (dt != null)
            {
                daily = dt;
                IsDeleteVisible = true; 
                TaskName = dt.Name;
                Completed = dt.Complete;
                if (Completed)
                {
                    SelectedStatus = StatusOptions[0];
                } else
                {
                    SelectedStatus = StatusOptions[1];
                }

                SaveButtonText = "Update Task";
            }

        }
        private async void DeleteTaskBtn()
        {
            if (_taskId > 0)
            {
                await db.DeleteAsync<DailyTask>(_taskId);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void AddTask()
        {
            TaskDate = DateTime.Parse(TaskDateString);
            if (_taskId > 0)
            {
                bool isComplete = SelectedStatus == "Complete";
                DailyTask dt = new DailyTask { Id = _taskId, Name = TaskName, Date = TaskDate, Complete = isComplete, AssociatedProjectId = daily.AssociatedProjectId };
                await db.UpdateAsync(dt);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                DailyTask dt = new DailyTask { Name = TaskName, Date = TaskDate, AssociatedProjectId = _projectId };
                await db.AddAsync(dt);
                await Shell.Current.GoToAsync("..");
            }

        }

        private string _saveButtonText = "Add Task";
        public string SaveButtonText
        {
            get => _saveButtonText;
            set
            {
                if (_saveButtonText != value)
                {
                    _saveButtonText = value;
                    OnPropertyChanged(nameof(SaveButtonText));
                }
            }
        }

        private bool _isDeleteVisible;

        public bool IsDeleteVisible
        {
            get => _isDeleteVisible;
            set
            {
                if (_isDeleteVisible != value)
                {
                    _isDeleteVisible = value;
                    OnPropertyChanged(nameof(IsDeleteVisible));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
