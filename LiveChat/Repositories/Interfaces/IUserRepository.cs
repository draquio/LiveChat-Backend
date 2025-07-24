using LiveChat.Entities;

namespace LiveChat.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetGuestByUsername(string username);
        Task AddUser(User user);
        Task SaveChangesAsync();
    }
}
