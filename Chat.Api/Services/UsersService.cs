using Fullstack.API.Data;
using Fullstack.API.Interfaces;
using Fullstack.API.Models;
using Fullstack.API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Fullstack.API.Services
{
    public class UsersService : IUsers
    {
        private readonly DataContext _context;

        public UsersService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users
                .Select(x => new User
                {
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<User> Get(long id)
        {
            return await _context.Users
                .Where(x => x.Id == id)
                .Select(x => new User
                {
                    Name = x.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(long id)
        {
            bool userFound = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = await _context.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

                if (user == null)
                {
                    scope.Dispose();
                }
                else
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    userFound = true;
                }
                scope.Complete();
            }

            return userFound;
        }

        public async Task<User> Add(User user)
        {
            var userNew = new User();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var hash = PasswordHasher.Hash(user.Password);

                userNew = new User()
                {
                    Name = user.Name,
                    Password = hash,
                    Image = user.Image
                };
                await _context.Users.AddAsync(userNew);
                await _context.SaveChangesAsync();

                scope.Complete();
            }

            return userNew;
        }

        public async Task<User> Update(User user, long id)
        {
            var userUpdate = new User();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var hash = PasswordHasher.Hash(user.Password);

                userUpdate = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

                userUpdate.Password = hash;
                userUpdate.Name = user.Name;

                await _context.SaveChangesAsync();

                scope.Complete();
            }

            return userUpdate;
        }
    }
}
