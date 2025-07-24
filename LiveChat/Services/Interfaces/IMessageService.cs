using LiveChat.Entities;

namespace LiveChat.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message> SaveGroupMessage(User sender, string content);
    }
}
