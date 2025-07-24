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

        public async Task<Message> SaveGroupMessage(User sender, string content)
        {
            try
            {
                Message message = new Message
                {
                    Content = content,
                    SenderId = sender.id,
                    RoomId = null,
                    SentAt = DateTime.UtcNow
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
    }
}
