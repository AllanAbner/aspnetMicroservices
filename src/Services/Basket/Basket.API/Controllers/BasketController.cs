using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketController> _logger;
        private readonly DiscountGrpcServices _discountGrpcServices;
        

        public BasketController(IBasketRepository repository, ILogger<BasketController> logger, DiscountGrpcServices discountGrpcServices)
        {
            _repository = repository;
            _logger = logger;
            _discountGrpcServices = discountGrpcServices;
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new Entities.ShoppingCart(userName));
        }

        /// <summary>
        /// UpdateBasket
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Entities.ShoppingCart), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] Entities.ShoppingCart shoppingCart)
        {
            foreach (var item in shoppingCart.Items)
            {
                var coupon = await _discountGrpcServices.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _repository.UpdateBasket(shoppingCart));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}