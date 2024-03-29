﻿using Microsoft.Maui.Controls.Compatibility;
using ProjectPlanner.Models;
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
        public ICommand TaskSelected { get; set; }
        public ICommand SprintViewBtn { get; set; }
        private Notes defaultNote { get; set; }
 

        private bool _isLoadingData = false;
        private DailyTask selectedTask;
        public ICommand SaveNoteCommand => new Command(SaveNotes);
        public DailyDisplayViewModel() {
            DailyTasksByDate = new ObservableCollection<DailyTask>();
            db = new DatabaseHelper();
            defaultNote = new Notes
            {
                Id = ProjectID,
                Date = DisplayDate,
                AssociatedProjectId = ProjectID,
                Note = ""
            };
            _currentNote = defaultNote;

            IncrementDate = new Command(IncrementDisplayDate);
            DecrementDate = new Command(DecrementDisplayDate);
            NavDailyTaskForm = new Command(NavToTaskForm);
            TaskSelected = new Command(OnTermSelected);
            SprintViewBtn = new Command(NavSprintView);


        }
        public DailyTask SelectedTask
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
        private void OnTermSelected()
        {

            if (selectedTask != null)
            {
                Shell.Current.GoToAsync($"dailyTaskForm?projectId={ProjectID}&taskDateString={DisplayDateString}&taskId={selectedTask.Id}");

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

        public DateTime DisplayDate 
        {
            get => displayDate;
            set
            {
                displayDate = value;
                OnPropertyChanged(nameof(DisplayDate));

                SelectDateChange();
            }
        }

        public bool IsLoadingData
        {
            get => _isLoadingData;
            set
            {
                _isLoadingData = value;
                OnPropertyChanged(nameof(IsLoadingData));
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

        public async void UpdateCompleteCommand(DailyTask task)
        {
            if (task != null)
            {
                task.Complete = task.Complete ? false : true;
                await db.UpdateAsync(task);

                await Load();
            }
        }

        public async Task Load()
        {
            _isLoadingData = true;
            DisplayDateString = DisplayDate.ToString("yyyy-MM-dd");
            Project p = await db.GetSingleAsync<Project>(_projectId);
            Notes n = await db.GetNotesByProjectAndDate(_projectId, DisplayDate);
            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(_projectId, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentProject = p;
                if(n != null && n.Note != null)
                {
                    _currentNote = n;
                    CurrentNote = n.Note;
                }
                else
                {
                    _currentNote.AssociatedProjectId = CurrentProject.Id;
                    CurrentNote = "";
                }
                OnPropertyChanged(nameof(CurrentProject));
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            });
            _isLoadingData = false;
        }

        private async void IncrementDisplayDate()
        {
            DisplayDate = DisplayDate.AddDays(1);
            DisplayDateString = DisplayDate.ToString("yyyy-MM-dd");

            Notes n = await db.GetNotesByProjectAndDate(_projectId, DisplayDate);
            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (n != null && n.Note != null)
                {
                    _currentNote = n;
                    CurrentNote = n.Note;
                }
                else
                {
                    _currentNote.AssociatedProjectId = CurrentProject.Id;
                    CurrentNote = "";
                }
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

            Notes n = await db.GetNotesByProjectAndDate(_projectId, DisplayDate);
            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (n != null && n.Note != null)
                {
                    _currentNote = n;
                    CurrentNote = n.Note;
                }
                else
                {
                    _currentNote.AssociatedProjectId = CurrentProject.Id;
                    CurrentNote = "";
                }
                GetTasksByDate.Clear();
                foreach (DailyTask dt in dbDailyTasks)
                {
                    GetTasksByDate.Add(dt);
                    OnPropertyChanged(nameof(GetTasksByDate));
                }

            }
            );
        }
        private void SelectDateChange()
        {
            SelectDateChangeAsync();
        }
        private async void SelectDateChangeAsync()
        {
            DisplayDateString = DisplayDate.ToString("yyyy-MM-dd");

            List<DailyTask> dbDailyTasks = await db.GetAllDailyByProjectIdAndDate(ProjectID, DisplayDate);
            Notes n = await db.GetNotesByProjectAndDate(_projectId, DisplayDate);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (n != null && n.Note != null)
                {
                    _currentNote = n;
                    CurrentNote = n.Note;
                }
                else
                {
                    _currentNote.AssociatedProjectId = CurrentProject.Id;
                    CurrentNote = "";
                }
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
        private async void NavSprintView()
        {
            await Shell.Current.GoToAsync($"sprintView?projectId={ProjectID}&taskDateString={DisplayDateString}");
        }

        private Notes _currentNote;

        public string CurrentNote
        {
            get => _currentNote.Note;
            set
            {

                _currentNote.Note = value;
                OnPropertyChanged(nameof(CurrentNote));
                
            }
        }

        public async void SaveNotes()
        {
            if (_currentNote.Id > -1)
            {
                await db.UpdateAsync(_currentNote);
            }
            else
            {
                await db.AddAsync(_currentNote);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
