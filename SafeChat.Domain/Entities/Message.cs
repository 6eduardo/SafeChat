using System;
using System.Collections.Generic;
using System.Text;

namespace SafeChat.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        //  Payload Encriptado (servidor nunca vê texto claro) 
        public string EncryptedContent { get; set; } = string.Empty;
        // ^ Texto encriptado com AES-256-CBC, armazenado em Base64
        public string EncryptedAesKey { get; set; } = string.Empty;
        // ^ Chave AES encriptada com RSA-OAEP público do destinatário
        public string AesIv { get; set; } = string.Empty;
        // ^ IV aleatório de 16 bytes em Base64 (único por mensagem)

        // Navigation Properties
        public Conversation Conversation { get; set; } = null!;
        public User Sender { get; set; } = null!;
    }
}
