using ChatAppServer.Models;
using System.Text.RegularExpressions;

namespace ChatAppServer.Utils
{
    public interface IMessageQueue
    {
        void InitQueue(string username);
        void QueueDirectMessage(Messege message, string username);
        void QueueMessageFor(Messege messege, string senderusername);
        void QueueGroupMessage(Messege messege, GroupChat group);
        Messege DequeueMessage(string username, string messageId, Messege msg = null!);
        void RemoveReceipt(string msgId, string username);
        void Clear(string username);
        Messege GetEarlisetMessage(string username);

        ThreadSafeList<Messege> GetMesseges(string username);
    }
}
