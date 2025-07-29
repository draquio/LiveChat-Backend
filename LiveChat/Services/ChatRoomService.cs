using LiveChat.Entities;
using LiveChat.Repositories;
using LiveChat.Repositories.Interfaces;
using LiveChat.Services.Interfaces;

namespace LiveChat.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomService(IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
        }

        public async Task<ChatRoom> GetOrCreatePrivateRoom(Guid? userId1, string? guest1, Guid? userId2, string? guest2)
        {
            ChatRoom? existingRoom = await _chatRoomRepository.GetPrivateRoom(userId1, guest1, userId2, guest2);
            if (existingRoom != null)
            {
                return existingRoom;
            }
            ChatRoom newRoom = new ChatRoom
            {
                IsPrivate = true,
                Participants = new List<ChatParticipant>()
                {
                    new ChatParticipant { UserId = userId1, GuestName = guest1 },
                    new ChatParticipant { UserId = userId2, GuestName = guest2 },
                },
            };
            await _chatRoomRepository.Create(newRoom);
            return newRoom;

        }
    }
}
