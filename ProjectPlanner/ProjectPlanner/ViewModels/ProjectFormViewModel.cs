using ProjectPlanner.Models;
using ProjectPlanner.Views;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(ProjectId), "projectId")]
    public class ProjectFormViewModel : INotifyPropertyChanged
    {
        
        public int _projectId = -1;

        private string projectName;

        public ICommand SaveProject { get; set; }
        public ICommand DeleteProject { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly DatabaseHelper db;

        public ProjectFormViewModel() {
            IsDeleteVisible = false;
            SaveProject = new Command(AddProject);
            DeleteProject = new Command(DeleteProjectBtn);
            db = new DatabaseHelper();
            if(_projectId > 0) { 
                RetrieveProject();
            }
        }


        public int ProjectId
        {
            set
            {
                _projectId = value;
            }
        }

        public string ProjectName
        {
            get => projectName; //Nice Lambda here to retrun the field as opposed to get {return termname}
            set
            {
                if (projectName != value) //value is a built in property
                {
                    projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }




        private Task Alert(string title, string message)
        {
            return Shell.Current.DisplayAlert(title, message, "OK");
        }
        private bool Validate()
        {

            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                Alert("Validation Error", "Project name is required.");
                return false;
            }

            return true;
        }


        private string _saveButtonText  = "Add Project";

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
        
        private async void DeleteProjectBtn()
        {
            if(_projectId > 0)
            {
                await db.DeleteAsync<Project>(_projectId);
                await Shell.Current.GoToAsync("mainPage");
            }
        }

        private async void AddProject()
        {

            bool isValid = Validate();  
            if (isValid) 
            { 
                if (_projectId > 0)
                {
                    Project p = new Project { Id = _projectId, Name = projectName };
                    await db.UpdateAsync(p);
                    await Shell.Current.GoToAsync("..");
                } else
                {


                    Project p = new Project { Name = projectName };
                    await db.AddAsync(p);
                    await Shell.Current.GoToAsync("..");
                }

            }
            
        }

        public async void RetrieveProject()
        {
            Project p = await db.GetSingleAsync<Project>(_projectId);
            if (p != null)
            {
                IsDeleteVisible = true;
                projectName = p.Name;
                OnPropertyChanged(nameof(ProjectName));
                
                _saveButtonText = "Update Project";
                OnPropertyChanged(nameof(SaveButtonText));
            }
           
        }
    
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
