using SoftwarePal.Models;
using SoftwarePal.Repositories;
using SoftwarePal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace SoftwarePal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public TeamsController(ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _teamService.GetById(id);
            return Ok(team);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _teamService.GetAll();
            return Ok(teams);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddTeam([FromForm] Team team)
        {
            if (team.Image != null)
                team.ImageName = await _teamService.SaveImage(team.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            team.UserCreated = user?.Id;
            await _teamService.Add(team);
            return Ok(team);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromForm] Team team)
        {
            if (team.Id != id)
            {
                return BadRequest();
            }
            if (team.Image != null)
                team.ImageName = await _teamService.SaveImage(team.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            team.UserUpdated = user?.Id;
            await _teamService.Update(team);
            return Ok(team);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _teamService.GetById(id);
            if (team == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _teamService.Delete(team);
            return Ok();
        }

    }
}
