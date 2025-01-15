using ChatAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Data.Repositories
{
    public class MessagesRepo : IMessagesRepo
    {
        public ChatAppDbContext dbContext { get; set; }
        public MessagesRepo(ChatAppDbContext context)
        {
            dbContext = context;
        }
        public async Task<Messege?> GetMessageByUnifiedId(string unifiedId)
        {
            return await dbContext.Messeges.FirstOrDefaultAsync(m => m.UnifiedId == unifiedId);
        }

        public async Task SetMessageReceived(string myUsername, Messege message)
        {
            if (message!.GroupChatId > 0)
            {
                MessageReceipt receipt = new MessageReceipt()
                {
                    Username = myUsername,
                    Message = message,
                    MessageId = message!.Id,
                    Received = true
                };
                message!.UpdatedAt = DateTime.UtcNow;
                message!.Receipts.Add(receipt);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                message!.Received = true;
                message.UpdatedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }
        }

        
    }
}
