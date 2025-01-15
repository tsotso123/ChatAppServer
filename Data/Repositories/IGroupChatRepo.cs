using ChatAppServer.Models;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;

namespace ChatAppServer.Data.Repositories
{
    public interface IGroupChatRepo
    {
        Task AddGroup(GroupChat group);
        GroupChat? GetGroupById(int id);
        bool BelongsToGroup(GroupChat group, string username);
        Task AddGroupMessage(Messege message, GroupChat group);
        List<Messege> GetMissedGroupMessages(Account account);
        List<GroupChat> GetMissedGroupsCreations(Account account);

    }
}
