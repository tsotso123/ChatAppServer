using ChatAppServer.Models;

namespace ChatAppServer.Data.Repositories
{
    public interface IMessagesRepo
    {
        Task<Messege?> GetMessageByUnifiedId(string unifiedId);
        Task SetMessageReceived(string myUsername, Messege message);

    }
}
