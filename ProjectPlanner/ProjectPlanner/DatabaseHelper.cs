using ProjectPlanner.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProjectPlanner.Interfaces;

namespace ProjectPlanner
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private SQLiteAsyncConnection _db;

        async Task Init()
        {
            if (_db is not null)
                return;

            _db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result1 = await _db.CreateTableAsync<Project>();
            var result2 = await _db.CreateTableAsync<DailyTask>();
            var result3 = await _db.CreateTableAsync<Notes>();
            var result4 = await _db.CreateTableAsync<Sprint>();
            var result5 = await _db.CreateTableAsync<SprintTask>();
        }

        public async Task<List<T>> GetAllAsync<T>() where T : new()
        {
            await Init();
            return await _db.Table<T>().ToListAsync();
        }

        public async Task<int> AddAsync<T>(T item) where T : new()
        {
            await Init();
            return await _db.InsertAsync(item);
        }

        public async Task<T> GetSingleAsync<T>(int key) where T : new()
        {
            await Init();
            return await _db.GetAsync<T>(key);
        }

        public async Task<int> UpdateAsync<T>(T item) where T : new()
        {
            await Init();
            return await _db.UpdateAsync(item);
        }

        public async Task<int> DeleteAsync<T>(int key) where T : new()
        {
            await Init();
            return await _db.DeleteAsync<T>(key);
        }

        public async Task<List<Project>> GetAllByNameAsync(string key)
        {
            await Init();
            return await _db.Table<Project>()
                    .Where(x => x.Name.Contains(key))
                    .ToListAsync();
        }

        public async Task Truncate()
        {
            await Init();
            await _db.ExecuteAsync("DELETE FROM Project");
            await _db.ExecuteAsync("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Project'");
            await _db.ExecuteAsync("DELETE FROM DailyTask");
            await _db.ExecuteAsync("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='DailyTask'");
            await _db.ExecuteAsync("DELETE FROM Notes");
            await _db.ExecuteAsync("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Notes'");
            await _db.ExecuteAsync("DELETE FROM Sprint");
            await _db.ExecuteAsync("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Sprint'");
            await _db.ExecuteAsync("DELETE FROM SprintTask");
            await _db.ExecuteAsync("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='SprintTask'");
        }

        public async Task Reseed()
        {
            await Init();
            await Truncate();
            DateTime date = DateTime.Now;
            // Projects
            Project p = new Project { Id = 1, Name = "Project Cool Name" };
            await AddAsync(p);
            Project p2 = new Project { Id = 2, Name = "Project Car" };
            await AddAsync(p2);
            Project p3 = new Project { Id = 3, Name = "Project Dairy" };
            await AddAsync(p3);

            //Daily Tasks
            DailyTask d1 = new() { Name = "Do it now", Complete = true, AssociatedProjectId = 1, Date = date };
            await AddAsync(d1);
            DailyTask d2 = new() { Name = "Do it never", Complete = true, AssociatedProjectId = 2, Date = date };
            await AddAsync(d2);
            DailyTask d3 = new() { Name = "Do it later", Complete = false, AssociatedProjectId = 1, Date = date };
            await AddAsync(d3);

            //Notes
            Notes n1 = new Notes { Note = "Today Is The Day", AssociatedProjectId = 1, Date = date };
            await AddAsync(n1);
            Notes n2 = new Notes { Note = "Today Is Yesterday", AssociatedProjectId = 1, Date = date.AddDays(-1) };
            await AddAsync(n2);

            //Sprints
            Sprint s1 = new Sprint { Name = "First Sprint", AssociatedProjectId = 1, StartDate = date, EndDate = date.AddDays(14) };
            await AddAsync(s1);

            //SprintTask
            SprintTask st1 = new SprintTask { Name = "SprintThis", AssociatedSprintId = 1, Complete = false };
            await AddAsync(st1);

        }

        //Custom Fields Here

        public async Task<List<DailyTask>> GetAllDailyByProjectId(int key)
        {
            await Init();
            var query = _db.Table<DailyTask>().Where(t => t.AssociatedProjectId == key);
            return await query.ToListAsync();

        }

        public async Task<List<DailyTask>> GetAllDailyByProjectIdAndDate(int key, DateTime date)
        {
            await Init();
            var allTasks = await _db.Table<DailyTask>().Where(t => t.AssociatedProjectId == key).ToListAsync();
            var filteredTasks = allTasks.Where(t => t.Date.Date == date.Date).ToList();

            return filteredTasks;
        }

        public async Task<Notes> GetNotesByProjectAndDate(int key, DateTime date)
        {
            await Init();
            var Notes = await _db.Table<Notes>().Where(t => t.AssociatedProjectId == key).ToListAsync();
            var note = Notes.FirstOrDefault(t => t.Date.Date == date.Date);

            return note;

        }

        public async Task<List<Sprint>> GetAllSprintsByProjectId(int key)
        {
            await Init();
            var query = _db.Table<Sprint>().Where(t => t.AssociatedProjectId == key);
            return await query.ToListAsync();
        }

        public async Task<List<SprintTask>> GetAllSprintTasksBySprintId(int key)
        {
            await Init();
            var query = _db.Table<SprintTask>().Where(t => t.AssociatedSprintId == key);
            return await query.ToListAsync();
        }
    }
}
