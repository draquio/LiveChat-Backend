using LiveChat.Context;
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _dbContext;

        public UserRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;

        }

        public Task AddUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetGuestByUsername(string username)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsGuest);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
