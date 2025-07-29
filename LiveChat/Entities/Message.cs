namespace LiveChat.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public Guid? SenderId { get; set; }
        public User? Sender { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public Guid? RoomId { get; set; }
        public ChatRoom? Room { get; set; }
        public MessageType Type { get; set; } = MessageType.Text;
    }
}
public enum MessageType
{
    Text,
    Image,
    Audio,
    Document
}