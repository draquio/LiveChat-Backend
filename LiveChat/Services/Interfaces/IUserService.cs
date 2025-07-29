using LiveChat.Entities;

namespace LiveChat.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetOrCreateGuestUser(string username);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByUsername(string username);
        Task<User> Create(User user);
    }
}
