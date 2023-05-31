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

        [HttpGet("MyCart")]
        public async Task<IActionResult> GetCartByUserId()
        {
            User user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found!" });
            try
            {
                Cart cart = await _cartService.GetCartByUserId(user.Id);
                return Ok(cart);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
        {
            
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            try
            {
                var cart = await _cartService.AddToCart(request.ItemId, request.Qty, user.Id);
                return Ok(cart);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartRequest request)
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            try
            {
                await _cartService.RemoveFromCart(request.ItemId, request.Qty, user.Id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

    }
}
