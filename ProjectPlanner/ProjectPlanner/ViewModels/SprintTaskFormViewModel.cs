using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(SprintId), "sprintId")]
    [QueryProperty(nameof(TaskId), "taskId")]
    public class SprintTaskFormViewModel : INotifyPropertyChanged
    {
        public int _taskId = -1;
        public int _projectId = -1;
        readonly DatabaseHelper db;
        private SprintTask task;

        private string taskName;
        private bool completed;

        public List<string> StatusOptions { get; set; }

        public ICommand SaveTask { get; set; }
        public ICommand DeleteTask { get; set; }


        public SprintTaskFormViewModel()
        {
            db = new DatabaseHelper();
            SaveTask = new Command(AddTask);
            DeleteTask = new Command(DeleteTaskBtn);
            StatusOptions = ["Complete", "Not Complete"];
            SelectedStatus = StatusOptions[1];
        }

        public int SprintId
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
        private Task Alert(string title, string message)
        {
            return Shell.Current.DisplayAlert(title, message, "OK");
        }
        private bool Validate()
        {

            if (string.IsNullOrWhiteSpace(TaskName))
            {
                Alert("Validation Error", "Task name is required.");
                return false;
            }

            return true;
        }
        public async void RetrieveTask()
        {
            SprintTask st = await db.GetSingleAsync<SprintTask>(_taskId);


            if (st != null)
            {
                task = st;
                IsDeleteVisible = true;
                TaskName = st.Name;
                Completed = st.Complete;
                if (Completed)
                {
                    SelectedStatus = StatusOptions[0];
                }
                else
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
                await db.DeleteAsync<SprintTask>(_taskId);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void AddTask()
        {
            bool valid = Validate();
            if (valid)
            {
                if (_taskId > 0)
                {
                    bool isComplete = SelectedStatus == "Complete";
                    SprintTask dt = new SprintTask { Id = _taskId, Name = TaskName, Complete = isComplete, AssociatedSprintId = task.AssociatedSprintId };
                    await db.UpdateAsync(dt);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    SprintTask dt = new SprintTask { Name = TaskName, AssociatedSprintId = _projectId };
                    await db.AddAsync(dt);
                    await Shell.Current.GoToAsync("..");
                }
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
