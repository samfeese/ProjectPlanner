using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ProjectPlanner.Models;


namespace ProjectPlanner.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    { 
        private ObservableCollection<Project> AllProjects { get; set; } // Provides the INotifyCollectionChanged interface, allowing UI update. I originally was going to just use a List<Terms> but MAUI as this collection
        public ICommand NavProjectForm { get; set; }
        public ICommand ProjectSelected { get; set; }
        public ICommand EditProject { get; set; }
        public ICommand SeedData { get; }
        public ICommand Truncate { get;  }
        private Project selectedProject;

        readonly DatabaseHelper db;

        public MainPageViewModel() 
        {
            AllProjects = new ObservableCollection<Project>(); 
            db = new DatabaseHelper();
            NavProjectForm = new Command(NavigateToProjectForm);
            ProjectSelected = new Command(OnProjectSelected);
            EditProject = new Command<Project>(OnEditTerm);
            SeedData = new Command(Seed);
            Truncate = new Command(TruncateData);

        }

        public ObservableCollection<Project> GetProjects
        {
            get => AllProjects;
            set
            {
                AllProjects = value;
                OnPropertyChanged(nameof(GetProjects));
            }
        }

        private async void NavigateToProjectForm()
        {
              await Shell.Current.GoToAsync("projectForm");
          
        }
        public Project SelectedProject
        {
            get => selectedProject; 
            set
            {
                if (selectedProject != value) 
                {
                    selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }
        private void OnProjectSelected()
        {

            if (selectedProject != null)
            {          
                Shell.Current.GoToAsync($"projectPage?projectId={SelectedProject.Id}");

            }
        }
       

        private async void OnEditTerm(Project p)
        {
            if (p != null)
            {
                SelectedProject = p;
                await Shell.Current.GoToAsync($"projectForm?projectId={SelectedProject.Id}");
            }
        }


        public async Task Load()
        {
            List<Project> dbProjects = await db.GetAllAsync<Project>();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetProjects.Clear();               
                foreach (Project p in dbProjects)
                {
                    GetProjects.Add(p);
                    OnPropertyChanged(nameof(GetProjects));
                }
                
            }
            );
        }

        public async void Seed()
        {
           await db.Reseed();
        await Shell.Current.GoToAsync($"mainPage");
        }

        public async void TruncateData()
        {
            await db.Truncate();
            await Shell.Current.GoToAsync($"mainPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
