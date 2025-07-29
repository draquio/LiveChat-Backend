using LiveChat.Entities;

namespace LiveChat.Repositories.Interfaces
{
    public interface IChatRoomRepository : IGenericRepository<ChatRoom>
    {
        Task<ChatRoom?> GetPrivateRoom(Guid? userId1, string? guest1, Guid? userId2, string? guest2);
    }
}
