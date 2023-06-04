using Microsoft.AspNetCore.Mvc;
using SoftwarePal.Services;
using System.Net;
using System.Text.Json;

namespace SoftwarePal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PayPalService _paypalService;

        public PaymentController(PayPalService paypalService)
        {
            _paypalService = paypalService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(Models.Order order)
        {
            var response = await _paypalService.CreateOrder(order);

            return Ok(response);
        }
        
    }

}
