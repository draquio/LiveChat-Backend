using LiveChat.Entities;

namespace LiveChat.Repositories.Interfaces
{
    public interface ITokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetValidToken(string token);
        Task Revoke(string token);
    }
}
