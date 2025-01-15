using ChatAppServer.Models;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace ChatAppServer.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<string> getPasswordForUsername(string username);
        Task<List<Account>> GetAccountsMatchingQuery(string usernameQuery);

        Task<Account?> GetAccountByUsername(string username);
        
        Task<List<Account>> GetAllAccountsByUsername(List<string> usernames);
        DirectChat? GetDirectChat(Account user1, Account user2);
        Task SetLastLogin(Account account, DateTime lastLogin);
        Task<List<DirectChat>> GetDirectChats(string username, int pageNumber, int pageSize);
    }
}
