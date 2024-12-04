namespace TaskManagementSystem.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string username, string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
