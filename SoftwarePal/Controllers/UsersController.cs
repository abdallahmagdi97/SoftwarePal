using SoftwarePal.Models;
using SoftwarePal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

            if (_userService.GetUserByEmail(user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email address is already in use.");
                return BadRequest(ModelState);
            }

            var createdUser = _userService.AddUser(user);

            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userService.GetUserByEmail(loginRequest.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid email address or password.");
                return BadRequest(ModelState);
            }
            bool passwordVerified = await _userService.VerifyPassword(user, loginRequest.Password);
            if (!passwordVerified)
            {
                ModelState.AddModelError("Email", "Invalid email address or password.");
                return BadRequest(ModelState);
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
                return NotFound();
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
                return NotFound();
            }
            _userService.DeleteUser(existingUser);

            return Ok();
        }
    }
}
