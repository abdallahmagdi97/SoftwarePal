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
            User user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found!" });
            Cart cart = await _cartService.GetCartByUserId(user.Id);
            
            return Ok(cart);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(int id)
        {
            var cart = await _cartService.GetById(id);
            if (cart == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Cart not found" });
            return Ok(cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var carts = await _cartService.GetAll();
            return Ok(carts);
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody] Cart cart)
        {
            await _cartService.Add(cart);
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            cart.UserCreated = user?.Id;
            await _cartService.SaveChanges();
            return Ok(cart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] Cart cart)
        {
            if (cart.Id != id)
            {
                return BadRequest();
            }
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            cart.UserUpdated = user?.Id;
            await _cartService.Update(cart);
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _cartService.GetById(id);
            if (cart == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Not Found" });
            }
            _cartService.Delete(cart);
            await _cartService.SaveChanges();
            return Ok();
        }

    }
}
