using LiveChat.Entities;

namespace LiveChat.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetOrCreateGuestUser(string username);
    }
}
