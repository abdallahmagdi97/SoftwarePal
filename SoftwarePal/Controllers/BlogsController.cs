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
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public BlogsController(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _blogService.GetById(id);
            return Ok(blog);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            var blogs = await _blogService.GetAll();
            return Ok(blogs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBlog([FromBody] Models.Blog blog)
        {
            await _blogService.Add(blog);
            return Ok(blog);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Models.Blog blog)
        {
            var existingBlog = await _blogService.GetById(id);
            if (existingBlog == null)
            {
                return NotFound();
            }
            if (blog.Id != id)
            {
                return BadRequest();
            }
            await _blogService.Update(blog);
            return Ok(blog);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }
            _blogService.Delete(blog);
            return Ok();
        }

    }
}
