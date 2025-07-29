using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using LiveChat.Services.Interfaces;

namespace LiveChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> SaveGroupMessage(string sender, string content, MessageType type, Guid? senderId)
        {
            try
            {
                Message message = new Message
                {
                    Content = content,
                    SenderId = senderId ?? null,
                    SenderUsername = sender,
                    RoomId = null,
                    Type = type,
                    SentAt = DateTime.UtcNow,
                };
                await _messageRepository.AddMessage(message);
                await _messageRepository.SaveChangesAsync();
                return message;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Message> SavePrivateMessage(Guid roomId, string sender, string content, MessageType type, Guid? senderId)
        {
            var message = new Message
            {
                SenderUsername = sender,
                Content = content,
                Type = type,
                SenderId = senderId ?? null,
                RoomId = roomId,
                SentAt = DateTime.UtcNow
            };
            await _messageRepository.AddMessage(message);
            await _messageRepository.SaveChangesAsync();
            return message;
        }
    }
}
