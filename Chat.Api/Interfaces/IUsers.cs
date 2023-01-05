using Fullstack.API.Models;

namespace Fullstack.API.Interfaces
{
    public interface IUsers
    {
        public Task<List<User>> GetAll();
        public Task<User> Add(User user);
        public Task<User> Get(long id);
        public Task<bool> Delete(long id);
        public Task<User> Update(User user, long id);
    }
}
