using BlogManagement.Abstract;
using BlogManagement.DTO.Request;
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
        public async Task<ActionResult<User>> GetSingleUser([FromRoute]int userId)
        {
            var result = await _userService.GetUserAsync(userId);
            return result == null ? NotFound($"User with id {userId} is not exist") : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] CreateUpdateUser user)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existingUser = await _userService.GetUserAsync(user.Email);
            if (existingUser != null) return BadRequest($"User with email {user.Email} already exist");
            var result = await _userService.AddUserAsync(user);
            return Ok(result);

        }
        
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser([FromBody] CreateUpdateUser user)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existingUser = await _userService.GetUserAsync(user.Email);
            if (existingUser == null) return BadRequest($"User with email {user.Email} is not exist");
            var result= await _userService.UpdateUserAsync(user);
            return Ok(result);
        }
    }
}