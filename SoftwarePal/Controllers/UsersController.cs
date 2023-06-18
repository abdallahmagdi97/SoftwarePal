using SoftwarePal.Models;
using SoftwarePal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace SoftwarePal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_userService.EmailExists(user.Email))
            {
                ModelState.AddModelError("Email", "Email address is already in use.");
                return BadRequest(ModelState);
            }

            _userService.AddUser(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userService.GetUserByEmail(loginRequest.Email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Invalid email address" } );
            }
            bool passwordVerified = await _userService.VerifyPassword(user, loginRequest.Password);
            if (!passwordVerified)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Invalid password" } );
            }

            var token = _userService.GenerateToken(user);

            return Ok(new { token });
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }

            return Ok(user);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUser = await _userService.GetCurrentUser(HttpContext.User);
            if (currentUser == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            user.UserUpdated = currentUser?.Id;
            await _userService.UpdateUser(user);

            return Ok(user);
        }
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _userService.DeleteUser(existingUser);

            return Ok();
        }
    }
}
