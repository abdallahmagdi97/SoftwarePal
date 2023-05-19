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
    public class SubItemsController : ControllerBase
    {
        private readonly ISubItemService _subItemService;

        public SubItemsController(ISubItemService subItemService)
        {
            _subItemService = subItemService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubItem(int id)
        {
            var subItem = await _subItemService.GetById(id);
            return Ok(subItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubItems()
        {
            var subItems = await _subItemService.GetAll();
            return Ok(subItems);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddSubItem([FromForm] SubItem subItem)
        {
            await _subItemService.Add(subItem);
            return Ok(subItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubItem(int id, [FromBody] SubItem subItem)
        {
            if (subItem.Id != id)
            {
                return BadRequest();
            }
            await _subItemService.Update(subItem);
            return Ok(subItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubItem(int id)
        {
            var subItem = await _subItemService.GetById(id);
            if (subItem == null)
            {
                return NotFound();
            }
            _subItemService.Delete(subItem);
            return Ok();
        }

    }
}
