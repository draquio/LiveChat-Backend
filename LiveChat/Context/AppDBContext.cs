using LiveChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Messages)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId);
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
