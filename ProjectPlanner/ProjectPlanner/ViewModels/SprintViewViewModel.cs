using AndroidX.DynamicAnimation;
using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{

    [QueryProperty(nameof(ProjectID), "projectId")]
    public class SprintViewViewModel : INotifyPropertyChanged
    {
        public int _projectId = -1;
        private Project _project { get; set; }
        readonly DatabaseHelper db;

        public ICommand SprintViewBtn { get; set; }
        public ICommand SprintAction {  get; set; }
        public ICommand NavSprintForm { get; set; }
        public ICommand EditSprint { get; set; }
        public Sprint _selectedSprint { get; set; }

        private ObservableCollection<Sprint> AllSprints { get; set; }
        public SprintViewViewModel()
        {
            db = new DatabaseHelper();
            AllSprints = new ObservableCollection<Sprint>();

            SprintViewBtn = new Command(NavSprintView);
            SprintAction = new Command(SprintSelect);
            NavSprintForm = new Command(NavToSprintForm);
            EditSprint = new Command<Sprint>(OnSprintEdit);

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

        public Project CurrentProject
        {
            get => _project;
            set
            {
                _project = value;
                OnPropertyChanged(nameof(CurrentProject));
            }
        }
        public ObservableCollection<Sprint> GetSprints
        {
            get => AllSprints;
            set
            {
                AllSprints = value;
                OnPropertyChanged(nameof(GetSprints));
            }
        }
        public Sprint SelectedSprint
        {
            get => _selectedSprint;
            set
            {
                _selectedSprint = value;
                OnPropertyChanged(nameof(SelectedSprint));
            }
        }

        public async Task Load()
        {
    
            Project p = await db.GetSingleAsync<Project>(_projectId);
            CurrentProject = p;
            List<Sprint> sprints = await db.GetAllSprintsByProjectId(_projectId);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                        
                AllSprints.Clear();
                foreach (Sprint s in sprints)
                {
                    AllSprints.Add(s);
                    OnPropertyChanged(nameof(AllSprints));
                }

            });
        }

        private async void SprintSelect()
        {
            if (_selectedSprint != null)
            {
               await Shell.Current.GoToAsync($"sprintPage?sprintId={SelectedSprint.Id}");
            }
        }

        private async void NavSprintView()
        {
            await Shell.Current.GoToAsync($"dailyDisplay?projectId={ProjectID}");
        }

        private async void NavToSprintForm()
        {
            await Shell.Current.GoToAsync($"sprintForm?projectId={ProjectID}");
        }
        private async void OnSprintEdit(Sprint s)
        {
            if (s != null)
            {
                await Shell.Current.GoToAsync($"sprintForm?projectId={ProjectID}&sprintId={s.Id}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
