using Microsoft.EntityFrameworkCore;
using SafeChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeChat.Infrastructure.Persistence
{
    public class SafeChatDbContext: DbContext
    {
        public SafeChatDbContext(DbContextOptions options)
               : base(options) { }
        
        //TABLES
        public DbSet<User> Users => Set<User>();
        public DbSet<PublicKey> PublicKeys => Set<PublicKey>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.HasIndex(u => u.Username).IsUnique();
                e.Property(u => u.Username).HasMaxLength(50).IsRequired();
                e.Property(u => u.Email).HasMaxLength(256).IsRequired();
                e.Property(u => u.PasswordHash).IsRequired();
            });
            //PublicKey
            modelBuilder.Entity<PublicKey>(e =>
            {
                e.HasKey(pk => pk.Id);
                e.HasOne(pk => pk.User)
                .WithOne(u => u.PublicKey)
                .HasForeignKey<PublicKey>(pk => pk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                e.Property(pk => pk.KeyValue).IsRequired();
            });
            // ConversationParticipant(chave composta)
            modelBuilder.Entity<ConversationParticipant>(e =>
            {
                e.HasKey(cp => new { cp.ConversationId, cp.UserId });
                e.HasOne(cp => cp.Conversation)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(cp => cp.User)
                .WithMany(u => u.ConversationParticipants)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //  Message 
            modelBuilder.Entity<Message>(e =>
            {
                e.HasKey(m => m.Id);
                e.HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
                e.Property(m => m.EncryptedContent).IsRequired();
                e.Property(m => m.EncryptedAesKey).IsRequired();
                e.Property(m => m.AesIv).IsRequired();
            });
        }
    }
}

