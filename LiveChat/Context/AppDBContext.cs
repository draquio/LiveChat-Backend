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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Messages)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
