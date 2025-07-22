namespace backend.Services
{
    public interface IUserInitializationService
    {
        Task InitializeUserAsync(int userId);
    }
}