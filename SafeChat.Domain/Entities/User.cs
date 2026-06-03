using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SafeChat.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = false;
        public DateTime? LastSeenAt { get; set; }
        
        // Navigation Properties
        public PublicKey? PublicKey { get; set; }
        public ICollection ConversationParticipants { get; set; } = [];
        public ICollection SentMessages { get; set; } = [];
    }
}
