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
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IUserService _userService;

        public ItemsController(IItemService itemService, IUserService userService)
        {
            _itemService = itemService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var item = await _itemService.GetById(id);
                return Ok(item);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _itemService.GetAll();
            return Ok(items);
        }
        [AllowAnonymous]
        [HttpGet("GetItemsByCategory")]
        public async Task<IActionResult> GetItemsByCategory([FromQuery] int categoryId)
        {
            var items = await _itemService.GetItemsByCategory(categoryId);
            return Ok(items);
        }
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] Item item)
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            item.UserCreated = user?.Id;
            await _itemService.Add(item);
            return Ok(item);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] Item item)
        {
            if (item.Id != id)
            {
                return BadRequest();
            }
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            item.UserUpdated = user?.Id;
            await _itemService.Update(item);
            return Ok(item);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _itemService.GetById(id);
            if (item == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _itemService.Delete(item);
            return Ok();
        }

    }
}
