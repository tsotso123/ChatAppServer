using ChatAppServer.Models;

namespace ChatAppServer.Data.Repositories
{
    public interface IDirectChatRepo
    {
        Task<DirectChat> CreateDirectChat(Account user1, Account user2);
        Task AddDirectMessage(DirectChat chat, Messege msg);
        List<Messege> GetMissedDirectMessages(Account user);


    }
}
