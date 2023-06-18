using SoftwarePal.Models;
using SoftwarePal.Repositories;
using SoftwarePal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwarePal.Models.Filter;
using PayPalCheckoutSdk.Orders;

namespace SoftwarePal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;
        private readonly IUserService _userService;

        public ContactUsController(IContactUsService contactUsService, IUserService userService)
        {
            _contactUsService = contactUsService;
            _userService = userService;
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var contactInquiries = await _contactUsService.GetAllContactInquiries();
                var contactInquiriesList = contactInquiries.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
                return Ok(new PagedResponse<List<ContactUs>>(contactInquiriesList, validFilter.PageNumber, validFilter.PageSize, contactInquiries.Count()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddContactUs(ContactUs contactInquiry)
        {
            try
            {
                await _contactUsService.AddContactInquiry(contactInquiry);
                return Ok(contactInquiry);
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead(int contactInquiryId)
        {
            try
            {
                await _contactUsService.MarkAsRead(contactInquiryId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = e.Message });
            }
        }
    }
}
