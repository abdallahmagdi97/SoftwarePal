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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsService _reviewService;
        private readonly IUserService _userService;

        public ReviewsController(IReviewsService reviewService, IUserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> ApproveReview(int reviewId)
        {
            try
            {
                await _reviewService.ApproveReview(reviewId);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviews();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet("GetCustomerReviews")]
        public async Task<IActionResult> GetReviews()
        {
            try
            {
                var reviews = await _reviewService.GetApprovedReviews();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddReview(Review review)
        {
            try
            {
                review.IsApproved = false;
                await _reviewService.AddReview(review);
                return Ok(review);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }
    }
}
