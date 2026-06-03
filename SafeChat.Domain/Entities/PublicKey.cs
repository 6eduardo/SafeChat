using System;
using System.Collections.Generic;
using System.Text;

namespace SafeChat.Domain.Entities
{
    public class PublicKey
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string KeyValue { get; set; } = string.Empty;
         // ^ RSA-2048 em Base64, ex: 'MIIBIjANBgkqhkiG9w0BAQEFAAOC...'

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
    }
}
