using System.Text.Json.Serialization;

namespace ChatAppServer.Models
{
    public class MessageReceipt
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int MessageId { get; set; }
        [JsonIgnore]
        public virtual Messege? Message { get; set; }

        // The user this receipt is for
        public string? Username { get; set; }

        // Whether the message has been received
        public bool Received { get; set; }
    }

}
