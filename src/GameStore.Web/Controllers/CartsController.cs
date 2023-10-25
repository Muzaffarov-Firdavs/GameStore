using GameStore.Service.Interfaces.Orders;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Cart() => View();
    }
}
