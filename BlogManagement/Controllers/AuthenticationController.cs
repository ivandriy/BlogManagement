using BlogManagement.Abstract;
using BlogManagement.DTO.Request;
using BlogManagement.DTO.Response;
using BlogManagement.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISystemClock _systemClock;
        private readonly JwtConfigOptions _config;

        public AuthenticationController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfigOptions> jwtOptions, ISystemClock systemClock)
        {
            _userManager = userManager;
            _systemClock = systemClock;
            _config = jwtOptions.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserRegistrationResponse>> RegisterUser([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string> { "User registration request is not valid" }
                });
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string> {"Email already registered"}
                });
            }

            var newUser = new IdentityUser{Email = request.Email, UserName = request.UserName};
            var isCreated = await _userManager.CreateAsync(newUser, request.Password);
            if (!isCreated.Succeeded)
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                });
            var jwtToken = GenerateToken(newUser);
            return Ok(new UserRegistrationResponse
            {
                Success = true,
                Token = jwtToken
            });

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string> { "User registration request is not valid" }
                });
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string> { "Invalid user login" }
                });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser,request.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Success = false,
                    Errors = new List<string> { "Invalid password" }
                });
            }
            var jwtToken = GenerateToken(existingUser);
            return Ok(new UserRegistrationResponse
            {
                Success = true,
                Token = jwtToken
            });
        }

        private string GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Secret);
            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = _systemClock.GetCurrentDateTime().DateTime.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDesc);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;

        }
    }
}