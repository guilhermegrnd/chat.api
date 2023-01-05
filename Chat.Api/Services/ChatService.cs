using Fullstack.API.Data;
using Fullstack.API.Interfaces;
using Fullstack.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Fullstack.API.Services
{
    public class ChatService : IChat
    {
        private readonly DataContext _context;

        public ChatService(DataContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessage(long userId, long ToUserId, Message message)
        {
            var newMessage = new Message();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                newMessage = new Message()
                {
                    UserId = userId,
                    ToUserId = ToUserId,
                    Type = message.Type,
                    Text = message.Text,
                    Image = message.Image,
                    Reply = message.Reply,
                    CreateAt = DateTime.UtcNow
                };
                await _context.Messages.AddAsync(newMessage);
                await _context.SaveChangesAsync();

                scope.Complete();
            }

            return newMessage;
        }

        public async Task<bool> DeleteMessage(long userId, long ToUserId, long messageId)
        {
            bool messageFound = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var message = await _context.Messages
                    .Where(x => 
                        x.Id == messageId
                        && x.UserId == userId
                        && x.ToUserId == ToUserId
                    )
                    .FirstOrDefaultAsync();

                if (message == null)
                {
                    scope.Dispose();
                }
                else
                {
                    _context.Messages.Remove(message);
                    await _context.SaveChangesAsync();

                    messageFound = true;
                }
                scope.Complete();
            }

            return messageFound;
        }

        public async Task<List<Message>> GetChatMessages(long userId, long ToUserId)
        {
            return await _context.Messages
                .Where(x => 
                    (
                        x.UserId == userId
                        && x.ToUserId == ToUserId
                    )
                    ||
                    (
                        x.UserId == ToUserId
                        && x.ToUserId == userId
                    )
                )
                .OrderByDescending(x => x.CreateAt)
                .ToListAsync();
        }

        public async Task<List<User>> GetChats(long userId)
        {
            //long[] userIds = await _context.Messages
            //    .Where(x =>
            //        x.ToUserId == userId
            //        || x.UserId == userId
            //    )
            //    .Select(x => x.User.Id)
            //    .Distinct()
            //    .ToArrayAsync();

            return await _context.Users
                .Where(x => 
                    //userIds.Contains(x.Id)
                    //&& 
                    x.Id != userId
                )
                .ToListAsync();
        }
    }
}
