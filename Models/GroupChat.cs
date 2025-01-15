namespace ChatAppServer.Models
{
    public class GroupChat
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public virtual ICollection<Account> Members { get; set; } = new HashSet<Account>();
        public virtual ICollection<Messege> Messages { get; set; } = new HashSet<Messege>();
        public int? ManagerId { get; set; }  // Foreign key
        public virtual Account? Manager { get; set; }  // Navigation property
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
