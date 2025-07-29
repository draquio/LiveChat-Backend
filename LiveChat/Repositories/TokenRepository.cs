using LiveChat.Context;
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Repositories
{
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        public TokenRepository(AppDBContext dbContext) : base(dbContext) {}

        public async Task<RefreshToken?> GetValidToken(string token)
        {
            RefreshToken? existToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.ExpiresAt > DateTime.UtcNow && !rt.IsRevoked);
            return existToken;
        }

        public async Task Revoke(string token)
        {
            RefreshToken? existToken  = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
            if(existToken != null)
            {
                existToken.IsRevoked = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
