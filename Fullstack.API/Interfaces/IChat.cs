using Fullstack.API.Models;

namespace Fullstack.API.Interfaces
{
    public interface IChat
    {
        public Task<List<User>> GetChats(long userId);
        public Task<List<Message>> GetChatMessages(long userId, long toUserId);
        public Task<bool> DeleteMessage(long userId, long toUserId, long messageId);
        public Task<Message> AddMessage(long userId, long toUserId, Message message);
    }
}
