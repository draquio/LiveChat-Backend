using LiveChat.Context;
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

        public Task AddUser(User user)
        {
            _dbContext.Users.Add(user);
            return Task.CompletedTask;
        }

        public async Task<User?> GetByEmail(string email)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User?> GetByUsername(string username)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<User?> GetGuestByUsername(string username)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }
        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
