using LiveChat.Services.Interfaces;

namespace LiveChat.Services
{
    public class FileCleanupService : IFileCleanupService
    {
        public Task EnsureLimit(string directoryPath, int maxFiles, int deleteCountIfExceeded = 1)
        {
            var dir = new DirectoryInfo(directoryPath);
            if (!dir.Exists) return Task.CompletedTask;

            var files = dir.GetFiles().OrderBy(f => f.CreationTimeUtc).ToList();
            if (files.Count < maxFiles) return Task.CompletedTask;

            var excess = files.Count - maxFiles;

            var toDelete = Math.Min(excess, deleteCountIfExceeded);
            foreach (var file in files.Take(toDelete))
            {
                try
                {
                    file.Delete();
                }
                catch{ throw; }
            }
            return Task.CompletedTask;
        }
    }
}
