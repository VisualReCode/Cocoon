using System.Threading.Tasks;
using BlazorCocoon.Server.Services;
using BlazorCocoon.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCocoon.Server.Controllers
{
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ShoppingCart _shoppingCart;

        public CartController(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        [HttpGet("count")]
        public async Task<ActionResult<CartCount>> Count()
        {
            var count = await _shoppingCart.GetCountAsync();
            return new CartCount
            {
                Count = count,
            };
        }
    }
}