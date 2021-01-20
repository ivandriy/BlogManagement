using BlogManagement.Abstract;
using BlogManagement.DTO;
using BlogManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        [Route("{userId}")]
        public async Task<User> GetSingleUser([FromRoute]int userId) => await _userService.GetUserAsync(userId);

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers() => await _userService.GetAllUsersAsync();

        [HttpPost]
        public async Task AddOrUpdateUser([FromBody] CreateUpdateUser user)
        {
            await _userService.AddOrUpdateUserAsync(user);
        }
    }
}