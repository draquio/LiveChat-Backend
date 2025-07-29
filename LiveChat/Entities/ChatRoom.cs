namespace LiveChat.Entities
{
    public class ChatRoom
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsPrivate { get; set; } = false;
        public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
        public ICollection<Message> Messages { get; set; }
    }

    public class ChatParticipant
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public string? GuestName { get; set; }
        public Guid ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
