namespace LiveChat.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
