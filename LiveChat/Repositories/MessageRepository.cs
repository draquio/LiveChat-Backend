using LiveChat.Context;
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;

namespace LiveChat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDBContext _dbContext;

        public MessageRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddMessage(Message message)
        {
            try
            {
                _dbContext.Messages.AddAsync(message);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
