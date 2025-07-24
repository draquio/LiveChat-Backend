namespace LiveChat.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public Guid SenderId { get; set; }
        public User Sender { get; set; }
        public Guid? RoomId { get; set; }
    }
}
