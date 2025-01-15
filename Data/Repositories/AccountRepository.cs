using System.Linq;
using System.Security.Principal;
using ChatAppServer.Data;
using ChatAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ChatAppDbContext dbContext;

        public AccountRepository(ChatAppDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<DirectChat>> GetDirectChats(string username, int pageNumber, int pageSize)
        {
            var account = await GetAccountByUsername(username);

            return await dbContext.DirectChats             
                .Where(dc =>  dc.Participants.Contains(account!)) // Assuming DirectChat has a reference to Account
                .OrderByDescending(dc => dc.LastMessageSentAt) // Sort by LastMessageSentAt
                //.Skip((pageNumber - 1) * pageSize)             // Skip for pagination
                //.Take(pageSize)                                // Take for pagination
                .ToListAsync();
        }

        public async Task<Account?> GetAccountByUsername(string username)
        {
            Account? result = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            return result;
        }

        public async Task<List<Account>> GetAccountsMatchingQuery(string usernameQuery)
        {
            return await dbContext.Accounts.Where(a => a.Username.Contains(usernameQuery)).ToListAsync();
            
        }

        public async Task<List<Account>> GetAllAccountsByUsername(List<string> usernames)
        {
            var members = await dbContext.Accounts.Where(a => usernames.Contains(a.Username)).ToListAsync();
            return members;
        }

        public DirectChat? GetDirectChat(Account user1, Account user2)
        {
            var directChatRecord = user1.DirectChats.FirstOrDefault((dc) =>
            {
                foreach (var participant in dc.Participants)
                {
                    if (participant.Username != user1.Username && participant.Username != user2.Username)
                    {
                        return false;
                    }
                }
                return true;

            });
            return directChatRecord;
        }

        public async Task<string> getPasswordForUsername(string username)
        {
            Account? result = await dbContext.Accounts.FirstAsync(x => x.Username == username);

            if (result != null)
            {
                return result.Password!;
            }
            else
            {
                return null!;
            }


        }

        public async Task SetLastLogin(Account account, DateTime lastLogin)
        {
            account.LastLogin = lastLogin;
            await dbContext.SaveChangesAsync();
        }
    }
}
