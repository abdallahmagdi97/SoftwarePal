﻿using SoftwarePal.Models;
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
            // Get the current user from the authentication token
            User user = await _userService.GetCurrentUser(HttpContext.User);

            // Return the licenses to the client
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicense(int id)
        {
            var license = await _licenseService.GetById(id);
            return Ok(license);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetLicenses()
        {
            var licenses = await _licenseService.GetAll();
            return Ok(licenses);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddLicense([FromBody] Models.License license)
        {
            await _licenseService.Add(license);
            return Ok(license);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, [FromBody] Models.License license)
        {
            var existingLicense = await _licenseService.GetById(id);
            if (existingLicense == null)
            {
                return NotFound();
            }
            if (license.Id != id)
            {
                return BadRequest();
            }

            await _licenseService.Update(license);
            return Ok(license);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            var license = await _licenseService.GetById(id);
            if (license == null)
            {
                return NotFound();
            }
            _licenseService.Delete(license);
            return Ok();
        }
       
    }
}
