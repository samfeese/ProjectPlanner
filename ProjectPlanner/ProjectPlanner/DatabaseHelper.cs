using ProjectPlanner.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectPlanner
{
    public class DatabaseHelper
    {
        private SQLiteAsyncConnection _db;

        async Task Init()
        {
            if (_db is not null)
                return;

            _db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result1 = await _db.CreateTableAsync<Project>();
            var result2 = await _db.CreateTableAsync<DailyTask>();
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

        public async Task Truncate()
        {
            await Init();
            await _db.ExecuteAsync("DELETE FROM Project");
            await _db.ExecuteAsync("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Project'");
        }

        public async Task Reseed()
        {
            await Init();
            await Truncate();
            Project p = new Project { Id = 1, Name = "Project Cool Name" };
            await AddAsync(p);
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
            var query = _db.Table<DailyTask>().Where(t => t.AssociatedProjectId == key).Where(t => t.Date.Date == date.Date );
            return await query.ToListAsync();
        }
    }
}
