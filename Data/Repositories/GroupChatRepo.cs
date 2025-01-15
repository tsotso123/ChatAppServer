using ChatAppServer.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ChatAppServer.Data.Repositories
{
    public class GroupChatRepo : IGroupChatRepo
    {
        public ChatAppDbContext dbContext { get; set; }
        public GroupChatRepo(ChatAppDbContext context) { 
            this.dbContext = context;
        }
        public async Task AddGroup(GroupChat group)
        {
            var creator = group.Manager;
            group.Members.Add(creator!); // Add the creator to the group

            dbContext.GroupChats.Add(group);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddGroupMessage(Messege message, GroupChat group)
        {
            group.Messages.Add(message);
            await dbContext.SaveChangesAsync();
        }

        public bool BelongsToGroup(GroupChat group, string username)
        {
            return group.Members.FirstOrDefault(m => m.Username == username) != null;
        }

        public GroupChat? GetGroupById(int groupId)
        {
            var group = dbContext.GroupChats.FirstOrDefault(g => g.Id == groupId);
            return group;
        }

        public List<Messege> GetMissedGroupMessages(Account account)
        {
            var groupMissedMsgs = account!.GroupChats
                    .Where(gc => gc.Messages.Any(m => m.SentAt >= account.LastLogin || m.UpdatedAt >= account.LastLogin)) // Filter chats with missed messages
                    .SelectMany(gc => gc.Messages
                        .Where(m => m.SentAt >= account.LastLogin || m.UpdatedAt >= account.LastLogin)) // Extract missed messages
                    .ToList();
            return groupMissedMsgs;
        }

        public List<GroupChat> GetMissedGroupsCreations(Account account)
        {
            var missedGroupJoins = account!.GroupChats
                    .Where(gc => gc.CreatedAt >= account.LastLogin)
                    .ToList();
            return missedGroupJoins;
        }
    }
}
