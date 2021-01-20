using BlogManagement.DTO;
using BlogManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(int id);
        Task AddOrUpdateUserAsync(CreateUpdateUser user);
    }
}