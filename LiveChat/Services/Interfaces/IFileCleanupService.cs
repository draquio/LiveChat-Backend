namespace LiveChat.Services.Interfaces
{
    public interface IFileCleanupService
    {
        Task EnsureLimit(string directoryPath, int maxFiles, int deleteCountIfExceeded);
    }
}
