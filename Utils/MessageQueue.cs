using ChatAppServer.Models;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace ChatAppServer.Utils
{
    public class MessageQueue : IMessageQueue
    {
        private ConcurrentDictionary<string, ThreadSafeList<Messege>> UsersMsgQueue { get; set; }
         
        public MessageQueue()
        {
            UsersMsgQueue = new ConcurrentDictionary<string, ThreadSafeList<Messege>>();
        }

        public void QueueDirectMessage(Messege message, string username)
        {
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);

            if (msgQueue != null) // if msgQueue is null, user is not connected
            {
                var msgForQueue = new Messege()
                {
                    UnifiedId = message.UnifiedId,
                    Content = message.Content,
                    SentAt = message.SentAt,
                    Sender = message.Sender,
                    RecipientUsername = username

                };

                msgQueue.Add(msgForQueue);

            }
        }

        public void QueueMessageFor(Messege messege, string senderusername)
        {
            UsersMsgQueue.TryGetValue(senderusername, out ThreadSafeList<Messege>? msgQueue);

            if (msgQueue != null) // if msgQueue is null, user is not connected
            {
                foreach (var msg in msgQueue)
                {
                    if (msg.UnifiedId == messege.UnifiedId)
                    {
                        msg.UpdatedAt = messege.UpdatedAt;
                        msg.Receipts = messege.Receipts;
                        msg.Received = messege.Received;
                        return;
                    }
                }

                var msgForQueue = new Messege()
                {
                    UnifiedId = messege.UnifiedId,
                    Content = messege.Content,
                    SentAt = messege.SentAt,
                    Sender = messege.Sender,
                    RecipientUsername = messege.RecipientUsername,
                    Receipts = messege.Receipts,
                    Received = messege.Received,
                    UpdatedAt = messege.UpdatedAt

                };

                msgQueue.Add(msgForQueue);

            }
        }

        public void QueueGroupMessage(Messege messege, GroupChat group)
        {
            foreach (var member in group.Members)
            {
                UsersMsgQueue.TryGetValue(member.Username, out ThreadSafeList<Messege>? msgQueue);

                if (msgQueue != null) // if msgQueue is null, user is not connected
                {
                    var msgForQueue = new Messege()
                    {
                        UnifiedId = messege.UnifiedId,
                        Content = messege.Content,
                        SentAt = messege.SentAt,
                        Sender = messege.Sender,
                        GroupChatId = messege.GroupChatId
                    };

                    msgQueue.Add(msgForQueue);

                }
            }
        }

        private Messege GetMessege(string username, string messageId)
        {
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);
            if (msgQueue != null)
            {

                foreach (var msg in msgQueue)
                {
                    if (msg.UnifiedId == messageId)
                    {
                        var msgResult = msg;
                        return msgResult;
                    }
                }

            }
            return null!;
        }
        public Messege DequeueMessage(string username, string messageId, Messege msg=null!)
        {
            var msgResult = msg ?? GetMessege(username, messageId);
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);
            if (msgQueue!=null)
            {
                msgQueue.Remove(msgResult);
            }
            return msgResult;


        }

        public void RemoveReceipt(string msgId, string username)
        {
            var msgResult = GetMessege(username, msgId);
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);
            foreach (var receipt in msgResult.Receipts)
            {
                if (receipt.Username == username)
                {
                    msgResult.Receipts.Remove(receipt);
                    if (msgResult.Receipts.Count == 0)
                    {
                        DequeueMessage(username,msgId,msgResult);
                    }
                }
            }
        }

        public ThreadSafeList<Messege> GetMesseges(string username)
        {
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);
            if (msgQueue != null)
            {
                return msgQueue;
            }
            return null!;
        }

        public Messege GetEarlisetMessage(string username)
        {
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);
            if (msgQueue != null && msgQueue.Count>0)
            {
                return msgQueue[0];
            }
            return null!;
        }
        public void Clear(string username)
        {
            UsersMsgQueue.TryGetValue(username, out ThreadSafeList<Messege>? msgQueue);
            if (msgQueue != null)
            {
                msgQueue.Clear();
                UsersMsgQueue.Remove(username,out _);
            }
        }

        public void InitQueue(string username)
        {
            UsersMsgQueue[username] = [];
        }
    }
}
