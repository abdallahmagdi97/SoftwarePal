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
using System.Reflection.Metadata;

namespace SoftwarePal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AboutUsController : ControllerBase
    {
        private readonly IAboutUsService _aboutUsService;
        private readonly IUserService _userService;
        public AboutUsController(IAboutUsService aboutUsService, IUserService userService)
        {
            _aboutUsService = aboutUsService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAboutUs(int id)
        {
            var aboutUs = await _aboutUsService.GetById(id);
            return Ok(aboutUs);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAboutUs()
        {
            var aboutUs = await _aboutUsService.GetAll();
            return Ok(aboutUs);
        }
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddAboutUs([FromForm] AboutUs aboutUs)
        {
            if (aboutUs.Image != null)
                aboutUs.ImageName = await _aboutUsService.SaveImage(aboutUs.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            aboutUs.UserCreated = user?.Id;
            await _aboutUsService.Add(aboutUs);
            return Ok(aboutUs);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAboutUs(int id, [FromForm] AboutUs aboutUs)
        {
            if (id != aboutUs.Id)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            if (aboutUs.Image != null)
                aboutUs.ImageName = await _aboutUsService.SaveImage(aboutUs.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            aboutUs.UserUpdated = user?.Id;
            await _aboutUsService.Update(aboutUs);
            return Ok(aboutUs);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAboutUs(int id)
        {
            var aboutUs = await _aboutUsService.GetById(id);
            if (aboutUs == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _aboutUsService.Delete(aboutUs);
            return Ok();
        }

    }
}
