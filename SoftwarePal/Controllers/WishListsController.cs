using SoftwarePal.Models;
using SoftwarePal.Repositories;
using SoftwarePal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoftwarePal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly IUserService _userService;

        public WishListsController(IWishListService wishListService, IUserService userService)
        {
            _wishListService = wishListService;
            _userService = userService;
        }

        //[Authorize]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetWishList(int id)
        //{
        //    var wishList = await _wishListService.GetById(id);
        //    return Ok(wishList);
        //}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWishLists()
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            var wishLists = await _wishListService.GetAll(user.Id);
            return Ok(wishLists);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddWishList(int itemId)
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            try
            {
                var wishList = await _wishListService.Add(user.Id, itemId);
                return Ok(wishList);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }

        [Authorize]
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteWishList(int itemId)
        {
            var user = await _userService.GetCurrentUser(HttpContext.User);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found" });
            try
            {
                await _wishListService.Delete(user.Id, itemId);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
            
            return Ok();
        }

    }
}
