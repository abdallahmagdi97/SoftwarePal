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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            var blogs = await _blogService.GetAll();
            return Ok(blogs);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddBlog([FromForm] Blog blog)
        {
            if (blog.Image != null)
                blog.ImageName = await _blogService.SaveImage(blog.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            blog.UserCreated = user?.Id;
            await _blogService.Add(blog);
            return Ok(blog);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromForm] Models.Blog blog)
        {
            if (blog.Id != id)
            {
                return BadRequest();
            }
            if (blog.Image != null)
                blog.ImageName = await _blogService.SaveImage(blog.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            blog.UserUpdated = user?.Id;
            await _blogService.Update(blog);
            return Ok(blog);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogService.GetById(id);
            if (blog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _blogService.Delete(blog);
            return Ok();
        }

    }
}
