﻿using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPlanner.ViewModels
{
    [QueryProperty(nameof(ProjectID), "projectId")]
    class DailyDisplayViewModel  : INotifyPropertyChanged
    {
        public int _projectId = -1;
        readonly DatabaseHelper db;
        private ObservableCollection<DailyTask> DailyTasksByDate { get; set; }
        private Project _project { get; set; }
        private DateTime displayDate = DateTime.Now;
        private string dateString = string.Empty;
        public ICommand IncrementDate { get; set; }
        public ICommand DecrementDate { get; set; }
        public ICommand NavDailyTaskForm { get; set; }

        public DailyDisplayViewModel() {
            DailyTasksByDate = new ObservableCollection<DailyTask>();
            db = new DatabaseHelper();

            IncrementDate = new Command(IncrementDisplayDate);
            DecrementDate = new Command(DecrementDisplayDate);
            NavDailyTaskForm = new Command(NavToTaskForm);


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

        public DateTime DisplayDate 
        {
            get => displayDate;
            set  
            {
                displayDate = value;
                OnPropertyChanged(nameof(DisplayDate));
            }
        }

        public string DisplayDateString
        {
            get => dateString;
            set
            {
                dateString = value;
                OnPropertyChanged(nameof(DisplayDateString));
            }
        }

        public ObservableCollection<DailyTask> GetTasksByDate
        {
            get => DailyTasksByDate;
            set
            {
                DailyTasksByDate = value;
                OnPropertyChanged(nameof(GetTasksByDate));
            }
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

        public async Task Load()
        {
            DisplayDateString = DisplayDate.ToString("yyyy-MM-dd");
            Project p = await db.GetSingleAsync<Project>(_projectId);

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(_projectId, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentProject = p;
                OnPropertyChanged(nameof(CurrentProject));
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            }
            );
        }

        private async void IncrementDisplayDate()
        {
            DisplayDate = DisplayDate.AddDays(1);
            DisplayDateString = DisplayDate.ToString("yyyy-MM-dd");

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            }
            );
        }

        private async void DecrementDisplayDate()
        {
            DisplayDate = DisplayDate.AddDays(-1);
            DisplayDateString = DisplayDate.ToString("yyyy-MM-dd");

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            }
            );
        }

        private async void NavToTaskForm()
        {
            if (GetTasksByDate.Count >= 10)
            {
                //await MaxCoursesAlert();
                return;
            }
            await Shell.Current.GoToAsync($"dailyTaskForm?projectId={ProjectID}&taskDateString={DisplayDateString}");

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
