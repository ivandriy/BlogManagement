using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.DTO;
using BlogManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Services
{
    public class UserService : IUserService
    {
        private readonly BlogDbContext _dbContext;

        public UserService(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _dbContext.Users.ToArrayAsync();

        public async Task<User> GetUserAsync(int id) => await _dbContext.Users.Where(u => u.Id == id).SingleOrDefaultAsync();

        public async Task AddOrUpdateUserAsync(CreateUpdateUser user)
        {
            var existingUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                // Update user
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
            }
            else
            {
                //Create new user
                var newUser = new User { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
                await _dbContext.Users.AddAsync(newUser);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}