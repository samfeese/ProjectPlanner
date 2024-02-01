using ProjectPlanner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(SprintId), "sprintId")]
    public class SprintPageViewModel : INotifyPropertyChanged
    {
        private int _sprintId;
        private ObservableCollection<SprintTask> AllSprints { get; set; }
        private Sprint _currentSprint {  get; set; }
        private SprintTask selectedTask;
        public ICommand TaskSelected { get; set; }
        public ICommand NavTaskForm { get; set; }
        readonly DatabaseHelper db;
        public SprintPageViewModel()
        {
            AllSprints = new ObservableCollection<SprintTask>();
            db = new DatabaseHelper();
            TaskSelected = new Command(OnTaskSelected);
            NavTaskForm = new Command(NavToTaskForm);

        }

        public int SprintId {
            get => _sprintId;
            set
            {
                _sprintId = value;
                OnPropertyChanged(nameof(SprintId));

            }
        }
        public SprintTask SelectedTask
        {
            get => selectedTask;
            set
            {
                if (selectedTask != value)
                {
                    selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                }
            }
        }

        public ObservableCollection<SprintTask> GetSprints
        {
            get => AllSprints;
            set
            {
                AllSprints = value;
                OnPropertyChanged(nameof(GetSprints));
            }
        }
        public Sprint CurrentSprint
        {
            get => _currentSprint;
            set
            {
                _currentSprint = value;
                OnPropertyChanged(nameof(CurrentSprint));
            }
        }

        public async void RetrieveSprintTasks()
        {
            Sprint s = await db.GetSingleAsync<Sprint>(SprintId);
            CurrentSprint = s;
            List<SprintTask> st = await db.GetAllSprintTasksBySprintId(SprintId);
            MainThread.BeginInvokeOnMainThread(() =>
            {

                AllSprints.Clear();
                foreach (SprintTask s in st)
                {
                    AllSprints.Add(s);
                    OnPropertyChanged(nameof(AllSprints));
                }

            });

        }

        private void OnTaskSelected()
        {

            if (selectedTask != null)
            {
                Shell.Current.GoToAsync($"sprintTaskForm?taskId={SelectedTask.Id}");

            }
        }

        private async void NavToTaskForm()
        {
            if (GetSprints.Count >= 10)
            {
                //await MaxCoursesAlert();
                return;
            }
            await Shell.Current.GoToAsync($"sprintTaskForm?sprintId={SprintId}");

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
