using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
//using Discount.API.Models;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscoundController : ControllerBase
    {
        private readonly IDiscountRepository repository;
        public DiscoundController(IDiscountRepository repository)
        {
            this.repository = repository;
        }

        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [HttpGet("{productName}", Name = "GetDiscount")]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetDiscount(string productName)
        {
            // TODO: Your code here
            var discount = await repository.GetDiscount(productName);

            return Ok(discount);
        }
        
        [HttpPost("")]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            // TODO: Your code here
            await repository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { coupon.ProductName }, coupon);
        }

        [HttpPut("")]
        public async Task<IActionResult> PutTModel([FromBody] Coupon coupon)
        {
            return Ok(await repository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        public async Task<ActionResult<bool>> DeleteTModelById(string productName)
        {
            return Ok(await repository.DeleteDiscount(productName));

        }
    }
}