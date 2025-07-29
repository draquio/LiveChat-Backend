using LiveChat.Context;
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Repositories
{
    public class ChatRoomRepository : GenericRepository<ChatRoom>, IChatRoomRepository
    {
        public ChatRoomRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ChatRoom?> GetPrivateRoom(Guid? userId1, string? guest1, Guid? userId2, string? guest2)
        {
            ChatRoom? existingRoom = await _dbContext.ChatRooms.Include(r => r.Participants)
                .FirstOrDefaultAsync(r => r.IsPrivate && 
                r.Participants.Any(p => p.UserId == userId1 || p.GuestName == guest1) &&
                r.Participants.Any(p => p.UserId == userId2 || p.GuestName == guest2));
            return existingRoom;
        }
    }
}
