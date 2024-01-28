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

        private string taskName;

        public ICommand SaveTask { get; set; }
        public ICommand DeleteTask { get; set; }


        public DailyTaskFormViewModel()
        {
            db = new DatabaseHelper();
            SaveTask = new Command(AddTask);
            DeleteTask = new Command(DeleteTaskBtn);
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

        public async void RetrieveTask()
        {
            DailyTask dt = await db.GetSingleAsync<DailyTask>(_taskId);
            if (dt != null)
            {
                //Fill in form fields here


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
                DailyTask dt = new DailyTask { Id = _taskId, Name = TaskName, Date = TaskDate};
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
