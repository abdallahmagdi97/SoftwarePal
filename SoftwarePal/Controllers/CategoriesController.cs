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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public CategoriesController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetById(id);
            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet("GetCategoryBySlug/{slug}")]
        public async Task<IActionResult> GetCategoryBySlug(string slug)
        {
            try
            {
                var category = await _categoryService.GetBySlug(slug);
                return Ok(category);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] Category category)
        {
            if (category.Image != null)
                category.ImageName = await _categoryService.SaveImage(category.Image);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            category.UserCreated = user?.Id;
            await _categoryService.Add(category);
            return Ok(category);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] Models.Category category)
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            category.UserUpdated = user?.Id;
            await _categoryService.Update(category);
            return Ok(category);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _categoryService.Delete(category);
            return Ok();
        }

    }
}
