using LiveChat.Entities;

namespace LiveChat.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(User user);
        Task SaveRefreshToken(RefreshToken token);
        Task<RefreshToken?> GetValidRefreshToken(string token);
        Task RevokeRefreshToken(string token);
    }
}
