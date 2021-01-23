using BlogManagement.DTO;
using BlogManagement.DTO.Request;
using BlogManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string email);
        Task<User> AddUserAsync(CreateUpdateUser user);
        Task<User> UpdateUserAsync(CreateUpdateUser user);
    }
}