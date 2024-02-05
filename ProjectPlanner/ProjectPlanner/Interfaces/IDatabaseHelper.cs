using ProjectPlanner.Models;

namespace ProjectPlanner.Interfaces
{
    public interface IDatabaseHelper
    {
        Task<int> AddAsync<T>(T item) where T : new();
        Task<int> DeleteAsync<T>(int key) where T : new();
        Task<List<T>> GetAllAsync<T>() where T : new();
        Task<List<Project>> GetAllByNameAsync(string key);
        Task<List<DailyTask>> GetAllDailyByProjectId(int key);
        Task<List<DailyTask>> GetAllDailyByProjectIdAndDate(int key, DateTime date);
        Task<List<Sprint>> GetAllSprintsByProjectId(int key);
        Task<List<SprintTask>> GetAllSprintTasksBySprintId(int key);
        Task<Notes> GetNotesByProjectAndDate(int key, DateTime date);
        Task<T> GetSingleAsync<T>(int key) where T : new();
        Task Reseed();
        Task Truncate();
        Task<int> UpdateAsync<T>(T item) where T : new();
    }
}