using ChatAppServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.Data
{
    public class Validator
    {
        public CentrallizedRepo Repos { get; set; }
        public Validator(CentrallizedRepo centrallizedRepo)
        {
            this.Repos = centrallizedRepo;
        }

        public async Task<GroupChat> CreateGroup(string myUsername, string groupName, List<string> usernames)
        {
            var creatorUsername = myUsername;
            if (string.IsNullOrEmpty(creatorUsername))
            {
                throw new HubException("bad request");
            }

            //var creator = validator.Accounts.FirstOrDefault(a => a.Username == creatorUsername);
            var creator = await Repos.AccountRepository.GetAccountByUsername(creatorUsername);
            if (creator == null)
            {
                throw new HubException("User not found");
            }

            // Validate the list of usernames
            //var members = await validator.Accounts.Where(a => usernames.Contains(a.Username)).ToListAsync();
            var members = await Repos.AccountRepository.GetAllAccountsByUsername(usernames);
            if (members.Count != usernames.Count)
            {
                throw new HubException("Some users were not found");
            }

            // Create the group
            var group = new GroupChat
            {
                GroupName = groupName,
                Manager = creator,
                ManagerId = creator.Id,
                CreatedAt = DateTime.UtcNow,
                Members = members
            };

            await Repos.GroupChatRepo.AddGroup(group);
            return group;

        }
        public async Task<bool> JoinGroup(string myUsername, string groupIdString)
        {
            _ = int.TryParse(groupIdString, out int groupId);
            var username = myUsername;
            if (string.IsNullOrEmpty(username))
            {
                throw new HubException("bad request");
            }

            //var account = validator.Accounts.FirstOrDefault(a => a.Username == username);
            var account = await Repos.AccountRepository.GetAccountByUsername(username);
            if (account == null)
                throw new HubException("User not found");

            //var group = validator.GroupChats.FirstOrDefault(g => g.Id == groupId);
            var group = Repos.GroupChatRepo.GetGroupById(groupId);
            if (group == null)
                throw new HubException("Group not found");

            //bool belongsToGroup = group.Members.FirstOrDefault(m => m.Username == username) != null;
            bool belongsToGroup = Repos.GroupChatRepo.BelongsToGroup(group, username);

            return belongsToGroup;
        }

        public async Task<Messege> SendGroupMessege(string myUsername,string groupIdString, string messege, string unifiedMsgId)
        {
            _ = int.TryParse(groupIdString, out int groupId);
            //var group = validator.GroupChats.FirstOrDefault(g => g.Id == groupId);
            var group = Repos.GroupChatRepo.GetGroupById(groupId);
            if (group == null)
                throw new HubException("Group not found");

            string senderUsername = myUsername;
            //var senderAccount = validator.Accounts.FirstOrDefault(a => a.Username == senderUsername);
            var senderAccount = await Repos.AccountRepository.GetAccountByUsername(senderUsername);
            if (senderAccount == null)
                throw new HubException("Unauthorized");

            // for the concurrent queue, if message is not reaching client
            var timeSent = DateTime.UtcNow;


            // was up here

            var msg = new Messege()
            {
                UnifiedId = unifiedMsgId,
                Content = messege,
                SenderId = senderAccount.Id,
                Sender = senderAccount,
                GroupChatId = group.Id,
                GroupChat = group,
                SentAt = timeSent
            };

            await Repos.GroupChatRepo.AddGroupMessage(msg, group);
            return msg;
        }

        public async Task<Messege> SendDirectMessage(string myUsername,string user, string messege, string msgIdForConfirmation)
        {
            if (string.IsNullOrEmpty(user))
                throw new HubException("bad request");

            string senderUsername = myUsername;
            //var senderAccount = validator.Accounts.FirstOrDefault(a => a.Username == senderUsername);
            var senderAccount = await Repos.AccountRepository.GetAccountByUsername(senderUsername);
            if (senderAccount == null)
                throw new HubException("Unauthorized");

            var account = await Repos.AccountRepository.GetAccountByUsername(user);
            if (account == null)
                throw new HubException("Recepient not found");


            DateTime timeSent = DateTime.UtcNow;

            var directChatRecord = Repos.AccountRepository.GetDirectChat(account, senderAccount);

            if (directChatRecord == null)
            {
                directChatRecord = await Repos.DirectChatRepo.CreateDirectChat(account, senderAccount);
            }

            var msg = new Messege()
            {
                Content = messege,
                SenderId = senderAccount.Id,
                Sender = senderAccount,
                DirectChatId = directChatRecord.Id,
                DirectChat = directChatRecord,
                SentAt = timeSent,
                UnifiedId = msgIdForConfirmation,
                RecipientUsername = user
            };

            await Repos.DirectChatRepo.AddDirectMessage(directChatRecord, msg);
            return msg;
        }

        public async Task<Messege> MessageConfirmation(string myUsername,string msgConfirmationIdString)
        {
            _ = int.TryParse(msgConfirmationIdString, out int msgConfirmationId);

            var realMsg = await Repos.MessagesRepo.GetMessageByUnifiedId(msgConfirmationIdString);
            await Repos.MessagesRepo.SetMessageReceived(myUsername, realMsg!);
            return realMsg!;
        }
    }
}
