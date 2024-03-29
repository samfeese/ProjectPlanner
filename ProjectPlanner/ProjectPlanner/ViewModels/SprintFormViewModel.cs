﻿using ProjectPlanner.Interfaces;
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
    [QueryProperty(nameof(ProjectId), "projectId")]
    public class SprintFormViewModel : INotifyPropertyChanged
    {
        readonly IDatabaseHelper db;
        readonly IAlertService alertService;
        private int _projectId;
        private int _sprintId;

        private string sprintName;
        private DateTime startDate;
        private DateTime endDate;
        public ICommand SaveSprint { get; set; }
        public ICommand DeleteSprint { get; set; }

        private bool _isDeleteVisible {  get; set; }

        public SprintFormViewModel()
        {
            IsDeleteVisible = false;
            SaveSprint = new Command(AddSprintBtn);
            DeleteSprint = new Command(DeleteSprintBtn);
            db = new DatabaseHelper();
            alertService = new AlertService();

            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(14);
        }
        public SprintFormViewModel(IDatabaseHelper dbInjectionForTest, IAlertService alert)
        {
            IsDeleteVisible = false;
            SaveSprint = new Command(AddSprintBtn);
            DeleteSprint = new Command(DeleteSprintBtn);
            db = dbInjectionForTest;
            alertService = alert;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(14);
        }

        public int ProjectId
        {
            get => _projectId;
            set
            {
                _projectId = value;
                OnPropertyChanged(nameof(ProjectId));

            }
        }

        public int SprintId
        {
            get => _sprintId;
            set
            {
                _sprintId = value;
                OnPropertyChanged(nameof(SprintId));
            }
        }

        public string SprintName
        {
            get => sprintName;
            set
            {
                if (sprintName != value) 
                {
                    sprintName = value;
                    OnPropertyChanged(nameof(SprintName));
                }
            }
        }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        public async void RetrieveSprint()
        {
            Sprint course = await db.GetSingleAsync<Sprint>(SprintId);
            if (course != null)
            {
                IsDeleteVisible = true;
                SprintName = course.Name;
                StartDate = course.StartDate;
                EndDate = course.EndDate;
                
                SaveButtonText = "Update Sprint";
            }

        }
        private async void AddSprintBtn()
        {

            bool validate = Validate();
            if (validate)
            {
                {

                }
                if (SprintId > 0)
                {
                    Sprint s = new Sprint { Id = SprintId, Name = sprintName, StartDate = startDate, EndDate = endDate, AssociatedProjectId = ProjectId };
                    await db.UpdateAsync(s);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    Sprint s = new Sprint { Name = sprintName, StartDate = startDate, EndDate = endDate, AssociatedProjectId = ProjectId };
                    await db.AddAsync(s);
                    await Shell.Current.GoToAsync("..");
                }
            }

        }
        private Task Alert(string title, string message)
        {
            return alertService.ShowAlert(title, message, "OK");
        }
        public bool Validate()
        {

            if (string.IsNullOrWhiteSpace(SprintName))
            {
                Alert("Validation Error", "Sprint name is required.");
                return false;
            }
            if (StartDate == default || EndDate == default)
            {
                Alert("Validation Error", "Start and End dates are required.");
                return false;
            }
            if (EndDate < StartDate)
            {
                Alert("Validation Error", "End date cannot be earlier than Start date.");
                return false;
            }

            return true;
        }
        private async void DeleteSprintBtn()
        {
            if (SprintId > 0)
            {
                await db.DeleteAsync<Sprint>(SprintId);
                await Shell.Current.GoToAsync("..");
            }

        }

        private string _saveButtonText = "Add Sprint";

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
