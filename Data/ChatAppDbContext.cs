using ChatAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Data
{
    public class ChatAppDbContext : DbContext
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<DirectChat> DirectChats { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<Messege> Messeges { get; set; }

        public DbSet<MessageReceipt> Receipts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string hashedPass = BCrypt.Net.BCrypt.HashPassword("pestera68");
            modelBuilder.Entity<Account>().HasData(
                new Account { FirstName="adam",LastName="baruch",Id = 1, Username = "tsotso123", Password = hashedPass },
                new Account { FirstName = "adam2", LastName = "baruch", Id = 2, Username = "tsotso", Password = hashedPass },
                new Account { FirstName = "adam3", LastName = "baruch", Id = 3, Username = "tsotso9", Password = hashedPass },
                new Account { FirstName = "adam4", LastName = "baruch", Id = 4, Username = "tso", Password = hashedPass }
            );


            // Configure many-to-many relationship between Account and DirectChat
            // we dont need this config, it does this automatically, but we want to change the join table name for something meaningful
            modelBuilder.Entity<DirectChat>()
                .HasMany(dc => dc.Participants)
                .WithMany(a => a.DirectChats)
                .UsingEntity(j => j.ToTable("DirectChatParticipants")); // Custom join table name
                


            // because group chats, and account, have both one to many, and many to many relationship, we need to manually configure

            // Configure the many-to-many relationship between GroupChat and Account (Members)
            modelBuilder.Entity<GroupChat>()
                .HasMany(g => g.Members)
                .WithMany(a => a.GroupChats)
                .UsingEntity(j => j.ToTable("GroupChatMembers"));

            // Configure the one-to-many relationship for the Manager (Admin) of the group
            modelBuilder.Entity<GroupChat>()
                .HasOne(g => g.Manager)
                .WithMany() // One Account can manage multiple groups, maybe to add group chats collection in the func
                .HasForeignKey(g => g.ManagerId)
                .OnDelete(DeleteBehavior.SetNull); // Set null if the manager is removed
        }

        

    }
}
