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
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        private readonly IUserService _userService;

        public LicenseController(ILicenseService licenseService, IUserService userService)
        {
            _licenseService = licenseService;
            _userService = userService;
        }

        [HttpGet("mylicenses")]
        public async Task<IActionResult> GetMyLicenses()
        {
            try
            {
                // Get the current user from the authentication token
                User user = await _userService.GetCurrentUser(HttpContext.User);
                List<Models.License> licenses = await _licenseService.GetMyLicenses(user.Id);
                // Return the licenses to the client
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("GetByItemId/{itemId}")]
        public async Task<IActionResult> GetByItemId(int itemId)
        {
            try
            {
                var license = await _licenseService.GetByItemId(itemId);
                return Ok(license);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetLicenses()
        {
            try
            {
                var licenses = await _licenseService.GetAll();
                return Ok(licenses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddLicense([FromBody] Models.License license)
        {
            try
            {
                var user = await _userService.GetCurrentUser(HttpContext.User);
                if (user == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
                license.UserCreated = user?.Id;
                await _licenseService.Add(license);
                return Ok(license);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, [FromBody] Models.License license)
        {
            try
            {
                if (license.Id != id)
                {
                    return BadRequest();
                }
                var user = await _userService.GetCurrentUser(HttpContext.User);
                if (user == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
                license.UserUpdated = user?.Id;
                await _licenseService.Update(license);
                return Ok(license);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            var license = await _licenseService.GetById(id);
            if (license == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _licenseService.Delete(license);
            return Ok();
        }
       
    }
}
