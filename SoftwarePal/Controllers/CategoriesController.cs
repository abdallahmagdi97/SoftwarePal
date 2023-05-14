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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetById(id);
            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] Category category)
        {
            await _categoryService.Add(category);
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Models.Category category)
        {
            var existingCategory = _categoryService.GetById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryService.Update(category);
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            _categoryService.Delete(category);
            return Ok();
        }

    }
}
