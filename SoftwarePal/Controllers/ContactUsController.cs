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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contactInquiries = await _contactUsService.GetAllContactInquiries();
                return Ok(contactInquiries);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddReview(ContactUs contactInquiry)
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
    }
}
