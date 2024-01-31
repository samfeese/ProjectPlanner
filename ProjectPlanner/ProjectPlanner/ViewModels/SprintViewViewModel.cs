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

        private ObservableCollection<Sprint> AllSprints { get; set; }
        public SprintViewViewModel()
        {
            db = new DatabaseHelper();
            AllSprints = new ObservableCollection<Sprint>();

            SprintViewBtn = new Command(NavSprintView);
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

        private async void NavSprintView()
        {
            await Shell.Current.GoToAsync($"dailyDisplay?projectId={ProjectID}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
