using SoftwarePal.Models;
using SoftwarePal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SoftwarePal.Controllers
{
    [Authorize]
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
            var user = _userService.GetUserByEmail(loginRequest.Email);

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

        [HttpPut("{id}")]
        public IActionResult Update(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = _userService.UpdateUser(user);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

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
