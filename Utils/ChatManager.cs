
using ChatAppServer.Data;
using ChatAppServer.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;


namespace ChatAppServer.Utils
{
    [Authorize(Roles = "LoggedInUser")]  
    public class ChatManager : Hub<IClientFunctions>
    {
        //private readonly ChatAppDbContext validator;
        private readonly Validator validator;

        private IMessageQueue messageQueue;
        
        private ConcurrentDictionary<string, CancellationTokenSource> usersDcToken;
        public ChatManager(Validator dbContext, MessageQueue messageQueue, ConcurrentDictionary<string, CancellationTokenSource> usersDcToken)
        {
            this.validator = dbContext;
            this.messageQueue = messageQueue;

            this.usersDcToken = usersDcToken;
            
        }

        public async Task CreateGroup(string groupName, List<string> usernames)
        {

            var group = await validator.CreateGroup(Context.User?.Identity?.Name!, groupName, usernames);

            // Add each member to the SignalR group
            foreach (var member in group.Members)
            {
                // need to add to queue
                await Clients.User(member.Username).JoinGroup(Context.User?.Identity?.Name!, group.Id, groupName);

            }


            
        }


        public async Task JoinGroup(string groupIdString)
        {

            bool belongsToGroup = await validator.JoinGroup(Context.User?.Identity?.Name!, groupIdString);
            if (belongsToGroup)
            {                
                await Groups.AddToGroupAsync(Context.ConnectionId, groupIdString);

                
            }
            else
            {
                throw new HubException("Unauthorized");
            }
            

            
        }

        public async Task SendGroupMessege(string groupIdString,string messege, string unifiedMsgId)
        {
            _ = int.TryParse(groupIdString,out int groupId);

            var msg = await validator.SendGroupMessege(Context.User?.Identity?.Name!,groupIdString,messege,unifiedMsgId);

            messageQueue.QueueGroupMessage(msg, msg.GroupChat!);

            await Clients.Group(groupIdString).ReceiveGroupMessage(Context.User?.Identity?.Name!, messege, (DateTime)msg.SentAt!, unifiedMsgId, groupId);
        }

        // A method to send a messege from client to server    
        public async Task SendDirectMessage(string user, string messege,string msgIdForConfirmation)
        {
            var msg = await validator.SendDirectMessage(Context.User?.Identity?.Name!, user, messege, msgIdForConfirmation);

            messageQueue.QueueDirectMessage(msg, user);

            await Clients.User(user).ReceiveMessage(Context.User?.Identity?.Name!, msg.Content, (DateTime)msg.SentAt!, msg.UnifiedId!);

            await Clients.User(msg.Sender.Username).MessageConfirmation(msgIdForConfirmation, (DateTime)msg.SentAt!);

        }
        // senders call this function 
        public void SentMessageWasReceived(string msgConfirmationIdString)
        {
            messageQueue.DequeueMessage(Context.User?.Identity?.Name!,msgConfirmationIdString);
        }
        public void SentGroupMessageWasReceived(string msgConfirmationIdString, string username)
        {
            messageQueue.RemoveReceipt(msgConfirmationIdString,username);
        }

        // all receivers call this function, to queue a 'received' message status update for the sender
        public async Task MessageConfirmation(string msgConfirmationIdString)
        {
            _ = int.TryParse(msgConfirmationIdString, out int msgConfirmationId);

            var realMsg = await validator.MessageConfirmation(Context.User?.Identity?.Name!,msgConfirmationIdString);
            if (realMsg!.Sender.Username != Context.User?.Identity?.Name!)
            {
                messageQueue.DequeueMessage(Context.User?.Identity?.Name!, msgConfirmationIdString);
                messageQueue.QueueMessageFor(realMsg, realMsg.Sender.Username);
                if (realMsg!.GroupChatId > 0)
                {
                    await Clients.User(realMsg.Sender.Username).SentGroupMessageWasReceived(msgConfirmationIdString, Context.User?.Identity?.Name!, (int)realMsg.GroupChatId);
                }
                else
                {
                    await Clients.User(realMsg.Sender.Username).SentMessageWasReceived(msgConfirmationIdString, Context.User?.Identity?.Name!);
                }
            }

        }

        //
        private async Task SendGameStart(string username,string withUsername)
        {
            await Clients.User(username).StartGame(withUsername);
        }
        public async Task AcceptInvite(string fromUser)
        {
            var currentUser = Context.User?.Identity?.Name!;
            // Create both tasks without awaiting them immediately
            var task1 = SendGameStart(fromUser, currentUser);
            var task2 = SendGameStart(currentUser, fromUser);

            // Await both tasks simultaneously
            await Task.WhenAll(task1, task2);
        }
        public async Task SendInvite(string user)
        {
            await Clients.User(user).ReceiveInvite(Context.User?.Identity?.Name!);
        }
        public async Task SpawnedUnit(string unit,string user)
        {
            await Clients.User(user).EnemySpawned(unit);
        }
        //




        public override async Task OnConnectedAsync()
        {          
            await base.OnConnectedAsync();
            // If a client reconnects, cancel the delay task if it's still running
            usersDcToken.TryGetValue(Context.User?.Identity?.Name!, out CancellationTokenSource? token);

            var username = Context.User?.Identity?.Name;
            var account = await validator.Repos.AccountRepository.GetAccountByUsername(username!);

            if (token != null) // user reconnected within time frame (15 seconds)
            {
                token.Cancel();
                usersDcToken.Remove(Context.User?.Identity?.Name!, out _);
                await Reconnect();
            }
            else
            {            
                var missedDirectMessages = validator.Repos.DirectChatRepo.GetMissedDirectMessages(account!);
                var missedGroupMessages = validator.Repos.GroupChatRepo.GetMissedGroupMessages(account!);
                var missedGroupJoins = validator.Repos.GroupChatRepo.GetMissedGroupsCreations(account!);
                
                foreach (var group in missedGroupJoins)
                {
                    await Clients.User(username!).JoinGroup(group.Manager!.Username, group.Id, group.GroupName);
                }
                var missedMessages = missedDirectMessages.Concat(missedGroupMessages);

                foreach (var message in missedMessages)
                {                    
                    if (message.UnifiedId == null)
                    {
                        continue;
                    }

                    if (message.GroupChatId > 0)
                    {
                        
                        if (message.Sender.Username.Equals(Context.User?.Identity?.Name!) && message.UpdatedAt >= account!.LastLogin) // this means message was received in the group, by at least 1 member
                        {
                            foreach (var msgReceipt in message.Receipts) // this will send all recipients, including those already received
                            {                                
                                await Clients.User(Context.User?.Identity?.Name!).SentGroupMessageWasReceived(message.UnifiedId, msgReceipt.Username!, (int)message.GroupChatId);
                            }
                            continue;
                        }

                        
                        await Clients.User(username!).ReceiveGroupMessage(message.Sender.Username, message.Content, (DateTime)message.SentAt!, message.UnifiedId, (int)message.GroupChatId);

                    }
                    else
                    {
                        if (message.Sender.Username.Equals(Context.User?.Identity?.Name!) && message.Received)
                        {
                            var recipient = message.RecipientUsername;


                            await Clients.User(Context.User?.Identity?.Name!).SentMessageWasReceived(message.UnifiedId, recipient!);
                            continue;
                        }

                        await Clients.User(username!).ReceiveMessage(message.Sender.Username, message.Content, (DateTime)message.SentAt!, message.UnifiedId);

                    }

                }

                
            }
            
            var userGroupChats = account!.GroupChats;

            foreach (var group in userGroupChats)
            {
                string groupId = group.Id.ToString();

                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

            }

            messageQueue.InitQueue(Context.User?.Identity?.Name!);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Create a new CancellationTokenSource for the disconnection
            CancellationTokenSource _cancellationTokenSource;

            if (usersDcToken.TryGetValue(Context.User?.Identity?.Name!, out CancellationTokenSource? tokenSource) && tokenSource != null) // this check is nesseccary for times when account logs in from multiple devices (creating multiple connected sessions, then, when disconnecting, this function will be called for each session)
            {
                _cancellationTokenSource = tokenSource;
            }
            else
            {
                _cancellationTokenSource = new CancellationTokenSource();
                usersDcToken[Context.User?.Identity?.Name!] = _cancellationTokenSource;
            }
            
                     
            try
            {
                // Wait for 15 seconds or until the task is cancelled               
                await Task.Delay(TimeSpan.FromSeconds(15), _cancellationTokenSource.Token);
                usersDcToken.Remove(Context.User?.Identity?.Name!, out _);

                string myUsername = Context.User?.Identity?.Name!;

                var myAccount = await validator.Repos.AccountRepository.GetAccountByUsername(myUsername);

                // If the task was not cancelled, delete pending msgs for user (they will get them from db when reconnecting)
                var msg = messageQueue.GetEarlisetMessage(Context.User?.Identity?.Name!);
                messageQueue.Clear(Context.User?.Identity?.Name!);

                if (msg != null)
                {
                    if (msg.Sender.Username.Equals(Context.User?.Identity?.Name!)) 
                        myAccount!.LastLogin = msg.UpdatedAt;
                    else
                        myAccount!.LastLogin = msg.SentAt;
                }
                else
                {
                    myAccount!.LastLogin = DateTime.UtcNow.AddSeconds(-15);
                }
                //await validator.SaveChangesAsync();
                await validator.Repos.AccountRepository.SetLastLogin(myAccount!, (DateTime)myAccount!.LastLogin!);

                
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled, so do nothing or handle cancellation if needed
                Console.WriteLine("Task was cancelled due to client reconnection");
            }


            await base.OnDisconnectedAsync(exception);
        }

        public async Task Reconnect()
        {
            var msgQueue = messageQueue.GetMesseges(Context.User?.Identity?.Name!);
            if (msgQueue == null)
            {
                return;
            }
            foreach (var msg in msgQueue)
            {
                if (msg.GroupChatId > 0) // this message was meant for a group
                {
                    if (msg.Sender.Username.Equals(Context.User?.Identity?.Name!)) 
                    {
                        foreach (var msgReceipt in msg.Receipts)
                        {
                            await Clients.User(Context.User?.Identity?.Name!).SentGroupMessageWasReceived(msg.UnifiedId!, msgReceipt.Username!, (int)msg.GroupChatId);
                        }
                        continue;
                    }

                    await Clients.User(Context.User?.Identity?.Name!).ReceiveGroupMessage(msg.Sender.Username, msg.Content, (DateTime)msg.SentAt!, msg.UnifiedId!, (int)msg.GroupChatId);

                }
                else
                {
                    if (msg.Received)
                    {
                        var recipient = msg.RecipientUsername;

                        await Clients.User(Context.User?.Identity?.Name!).SentMessageWasReceived(msg.UnifiedId!, recipient!);

                        continue;
                    }

                    await Clients.User(Context.User?.Identity?.Name!).ReceiveMessage(msg.Sender.Username, msg.Content, (DateTime)msg.SentAt!, msg.UnifiedId!);
                }               

            }
        }
    }
}
