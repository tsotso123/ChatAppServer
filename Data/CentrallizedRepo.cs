using ChatAppServer.Data.Repositories;

namespace ChatAppServer.Data
{
    public class CentrallizedRepo
    {
        public IAccountRepository AccountRepository { get; set; }
        public IDirectChatRepo DirectChatRepo { get; set; }
        public IGroupChatRepo GroupChatRepo { get; set; }
        public IMessagesRepo MessagesRepo { get; set; }
        public CentrallizedRepo(IAccountRepository accountRepository, IDirectChatRepo directChatRepo, IGroupChatRepo groupChatRepo, IMessagesRepo messagesRepo)
        {
            this.AccountRepository = accountRepository;
            this.DirectChatRepo = directChatRepo;
            this.GroupChatRepo = groupChatRepo;
            this.MessagesRepo = messagesRepo;
        }
    }
}
