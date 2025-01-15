namespace ChatAppServer.Models
{
    public class DirectChat
    {
        public int Id { get; set; }
        public DateTime LastMessageSentAt { get; set; }
        public virtual ICollection<Account> Participants { get; set; } = new HashSet<Account>(); // Both users in the chat
        public virtual ICollection<Messege> Messages { get; set; } = new HashSet<Messege>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
