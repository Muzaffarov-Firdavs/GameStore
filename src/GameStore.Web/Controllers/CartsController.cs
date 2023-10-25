using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Orders;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class CartsController : Controller
    {
        public int CountGames = 5;

        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Cart() => View();

        //public async Task<IActionResult> AddCart(GameResultDto enity)
        //{

        //}
    }
}
