using LiveChat.Entities;

namespace LiveChat.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetGuestByUsername(string username);
        Task AddUser(User user);
        Task SaveChangesAsync();
        Task<User?> GetByEmail(string email);
        Task<User?> GetByUsername(string username);
    }
}
