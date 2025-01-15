using System.Security.Principal;

namespace ChatAppServer.Utils
{
    public interface IClientFunctions
    {
        Task JoinGroup(string username, int groupId, string groupName);
        Task ReceiveGroupMessage(string username, string message, DateTime timeSent, string unifiedMsgId, int groupIdString);

        Task ReceiveMessage(string username, string message, DateTime timeSent, string unifiedMsgId);

        Task MessageConfirmation(string msgIdForConfirmation, DateTime timeSent);

        Task SentGroupMessageWasReceived(string msgConfirmationIdString, string username,int groupChatId);

        Task SentMessageWasReceived(string msgConfirmationIdString, string username);

        Task SendInvite(string UsernameTo);
        Task ReceiveInvite(string FromUsername);

        Task AcceptInvite(string FromUsername);

        Task StartGame(string fromUser);
        Task EnemySpawned(string unit);

    }
}
