using LiveChat.Entities;

namespace LiveChat.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        Task SaveChangesAsync();
    }
}
