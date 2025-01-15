using ChatAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Data.Repositories
{
    public class DirectChatRepo : IDirectChatRepo
    {
        public ChatAppDbContext dbContext { get; set; }
        public DirectChatRepo(ChatAppDbContext context) {
            this.dbContext = context;
        }
        public async Task AddDirectMessage(DirectChat directChatRecord, Messege msg)
        {
            directChatRecord.LastMessageSentAt = (DateTime)msg.SentAt!;
            directChatRecord.Messages.Add(msg);
            await dbContext.SaveChangesAsync();
        }

        public async Task<DirectChat> CreateDirectChat(Account user, Account secondUser)
        {
            Account user1;
            Account user2;
            if (user.Id > secondUser.Id)
            {
                user1 = user;
                user2 = secondUser;
            }
            else
            {
                user1 = secondUser;
                user2 = user;
            }

            var directChatRecord = new DirectChat() { Participants = new HashSet<Account>() { user1, user2 }, CreatedAt = DateTime.UtcNow };
            dbContext.DirectChats.Add(directChatRecord);
            await dbContext.SaveChangesAsync(); 
            return directChatRecord;
        }

        public List<Messege> GetMissedDirectMessages(Account user)
        {
            var missedMessages = user!.DirectChats
                    .Where(dc => dc.Messages.Any(m => m.SentAt >= user.LastLogin || m.UpdatedAt >= user.LastLogin)) // Filter chats with missed messages
                    .SelectMany(dc => dc.Messages
                        .Where(m => m.SentAt >= user.LastLogin || m.UpdatedAt >= user.LastLogin)) // Extract missed messages
                    .ToList();

            return missedMessages;
        }
    }
}
