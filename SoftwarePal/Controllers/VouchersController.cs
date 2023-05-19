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
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly IUserService _userService;

        public VouchersController(IVoucherService voucherService, IUserService userService)
        {
            _voucherService = voucherService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucher(int id)
        {
            var voucher = await _voucherService.GetById(id);
            return Ok(voucher);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetVouchers()
        {
            var vouchers = await _voucherService.GetAll();
            return Ok(vouchers);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddVoucher([FromBody] Models.Voucher voucher)
        {
            await _voucherService.Add(voucher);
            return Ok(voucher);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, [FromBody] Models.Voucher voucher)
        {
            if (voucher.Id != id)
            {
                return BadRequest();
            }
            await _voucherService.Update(voucher);
            return Ok(voucher);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = await _voucherService.GetById(id);
            if (voucher == null)
            {
                return NotFound();
            }
            _voucherService.Delete(voucher);
            return Ok();
        }

    }
}
