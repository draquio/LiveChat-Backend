using LiveChat.Entities;

namespace LiveChat.Services.Interfaces
{
    public interface IChatRoomService 
    {
        Task<ChatRoom> GetOrCreatePrivateRoom(Guid? userId1, string? guest1, Guid? userId2, string? guest2);
    }
}
