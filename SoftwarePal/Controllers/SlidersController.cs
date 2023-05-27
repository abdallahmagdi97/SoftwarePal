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
    public class SlidersController : ControllerBase
    {
        private readonly ISliderService _sliderService;
        private readonly IUserService _userService;

        public SlidersController(ISliderService sliderService, IUserService userService)
        {
            _sliderService = sliderService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlider(int id)
        {
            var slider = await _sliderService.GetById(id);
            return Ok(slider);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetSliders()
        {
            var sliders = await _sliderService.GetAll();
            return Ok(sliders);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddSlider([FromForm] Slider slider)
        {
            if (slider.Image != null)
                slider.ImageName = await _sliderService.SaveImage(slider.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" } );
            slider.UserCreated = user?.Id;
            await _sliderService.Add(slider);
            return Ok(slider);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlider(int id, [FromForm] Slider slider)
        {
            if (slider.Id != id)
            {
                return BadRequest();
            }
            if (slider.Image != null)
                slider.ImageName = await _sliderService.SaveImage(slider.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            slider.UserUpdated = user?.Id;
            await _sliderService.Update(slider);
            return Ok(slider);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var slider = await _sliderService.GetById(id);
            if (slider == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _sliderService.Delete(slider);
            return Ok();
        }

    }
}
