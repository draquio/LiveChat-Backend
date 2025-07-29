using LiveChat.Entities;

namespace LiveChat.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message> SaveGroupMessage(string sender, string content, MessageType type, Guid? senderId);
        Task<Message> SavePrivateMessage(Guid roomId, string sender, string content, MessageType type, Guid? senderId);
    }
}
