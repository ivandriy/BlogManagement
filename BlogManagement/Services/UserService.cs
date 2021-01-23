using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.DTO.Request;
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

        public async Task<User> GetUserAsync(int id) => await _dbContext.Users.Include(u => u.Posts).Where(u => u.UserId == id).SingleOrDefaultAsync();
        
        public async Task<User> GetUserAsync(string email) => await _dbContext.Users.Include(u => u.Posts).Where(u => u.Email == email).SingleOrDefaultAsync();

        public async Task<User> AddUserAsync(CreateUpdateUser user)
        {
            var newUser = new User { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return newUser;
        }
        public async Task<User> UpdateUserAsync(CreateUpdateUser user)
        {
            var existingUser = await _dbContext.Users.Include(u => u.Posts).SingleOrDefaultAsync(u => u.Email == user.Email);
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            await _dbContext.SaveChangesAsync();
            return existingUser;
        }
    }
}