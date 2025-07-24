namespace LiveChat.Entities
{
    public class User
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null;
        public bool IsGuest { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Message> Messages { get; set; }

    }
}
