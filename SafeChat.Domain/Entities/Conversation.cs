using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SafeChat.Domain.Entities
{
    public class Conversation
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastMessageAt { get; set; }

        // Navigation Properties
        public ICollection Participants { get; set; } = [];
        public ICollection Messages { get; set; } = [];
    }
}
