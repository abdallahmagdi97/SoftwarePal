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
        public async Task<IActionResult> AddSlider([FromBody] Slider slider)
        {
            await _sliderService.Add(slider);
            return Ok(slider);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlider(int id, [FromBody] Slider slider)
        {
            var existingSlider = await _sliderService.GetById(id);
            if (existingSlider == null)
            {
                return NotFound();
            }
            if (slider.Id != id)
            {
                return BadRequest();
            }
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
                return NotFound();
            }
            _sliderService.Delete(slider);
            return Ok();
        }

    }
}
