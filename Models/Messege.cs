using System.Text.Json.Serialization;

namespace ChatAppServer.Models
{
    public class Messege
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        [JsonIgnore]
        public int SenderId { get; set; }  // Foreign key

        [JsonIgnore]
        public virtual Account Sender { get; set; } = null!;
        public string? RecipientUsername { get; set; }
        public int? GroupChatId { get; set; }  // Foreign key for group messages

        [JsonIgnore]
        public virtual GroupChat? GroupChat { get; set; }
        [JsonIgnore]
        public int? DirectChatId { get; set; }  // Foreign key for direct messages
        [JsonIgnore]
        public virtual DirectChat? DirectChat { get; set; }
        public DateTime? SentAt { get; set; }       
        public string? UnifiedId { get; set; } // 
        public bool Seen { get; set; } // for direct chat
        public bool Received { get; set; } // for direct chat
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<MessageReceipt> Receipts { get; set; } = new HashSet<MessageReceipt>();

    }
}
