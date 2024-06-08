using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        // POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Length > 0)
                {
                   identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                   if (identityResult.Succeeded)
                   {
                       return Ok("User was registered! Please login.");
                   }
                }
            }

            //return BadRequest(identityResult.Errors);
            return BadRequest("Something went wrong");
        }

        // POST: api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
            
            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for user
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create token

                        var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponceDto()
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }
            
            return BadRequest("Username or password incorrect");
        }
    }
}
