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
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;

        public CartsController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpGet("GetCartByUserId")]
        public async Task<IActionResult> GetCartByUserId()
        {
            // Get the current user from the authentication token
            User user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return NotFound("User not found!");
            Cart cart = await _cartService.GetCartByUserId(user.Id);
            // Return the licenses to the client
            return Ok(cart);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(int id)
        {
            var cart = await _cartService.GetById(id);
            if (cart == null)
                return NotFound("Cart not found");
            return Ok(cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var carts = await _cartService.GetAll();
            return Ok(carts);
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody] Models.Cart cart)
        {
            await _cartService.Add(cart);
            await _cartService.SaveChanges();
            return Ok(cart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] Models.Cart cart)
        {
            var existingCart = await _cartService.GetById(id);
            if (existingCart == null)
            {
                return NotFound();
            }
            if (cart.Id != id)
            {
                return BadRequest();
            }
            await _cartService.Update(cart);
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _cartService.GetById(id);
            if (cart == null)
            {
                return NotFound();
            }
            _cartService.Delete(cart);
            await _cartService.SaveChanges();
            return Ok();
        }

    }
}