using MailTask.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MailTask.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MailMessage> MailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MailMessage>(entity =>
            {
                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.Sent)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.Inbox)
                    .HasForeignKey(d => d.RecipientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            base.OnModelCreating(builder);
        }
    }
}