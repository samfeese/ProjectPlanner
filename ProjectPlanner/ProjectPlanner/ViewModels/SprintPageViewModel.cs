using ProjectPlanner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(SprintId), "sprintId")]
    public class SprintPageViewModel : INotifyPropertyChanged
    {
        private int _sprintId;
        private ObservableCollection<SprintTask> AllSprints { get; set; }
        private Sprint _currentSprint {  get; set; }

        readonly DatabaseHelper db;
        public SprintPageViewModel()
        {
            AllSprints = new ObservableCollection<SprintTask>();
            db = new DatabaseHelper();

        }

        public int SprintId {
            get => _sprintId;
            set
            {
                _sprintId = value;
                OnPropertyChanged(nameof(SprintId));

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
