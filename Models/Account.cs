using System.Text.Json.Serialization;

namespace ChatAppServer.Models
{
    public class Account
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        [JsonIgnore]
        public string Password { get; set; } = string.Empty;  // Hashed password

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? LastLogin { get; set; }

        // Navigation properties

        [JsonIgnore]
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new HashSet<GroupChat>();

        [JsonIgnore]
        public virtual ICollection<DirectChat> DirectChats { get; set; } = new HashSet<DirectChat>();
    }
}
