namespace LiveChat.Entities
{
    public class ChatRoom
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string name { get; set; }
        public bool IsPrivate { get; set; } = false;
        public ICollection<Message> Messages { get; set; }
    }
}
