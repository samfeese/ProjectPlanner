namespace ProjectPlanner.Interfaces
{
    public interface IAlertService
    {
        Task ShowAlert(string title, string message, string btnText);
    }
}