using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwarePal.Models;
using SoftwarePal.Services;
using System.Data;

namespace SoftwarePal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly IItemService _itemService;
        public OrdersController(IUserService userService, ICartService cartService, ICartItemService cartItemService, IItemService itemService)
        {
            _userService = userService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _itemService = itemService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Voucher voucher)
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            var cart = await _cartService.GetCartByUserId(user.Id);
            var cartItems = await _cartItemService.GetCartItemsByCartId(cart.Id);
            decimal subTotal = 0;
            decimal total = 0;
            foreach(var cartItem in cartItems)
            {
                var item = await _itemService.GetById(cartItem.ItemId);
                decimal itemPrice = await _itemService.GetPricefromPriceRole();
                subTotal += itemPrice; 
            }
            if (voucher != null)
            {
                total = subTotal - voucher.Amount;
            }else
            {
                total = subTotal;
            }
            return Ok();
        }
    }
}
