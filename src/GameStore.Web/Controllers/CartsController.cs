using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Interfaces.Orders;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class CartsController : Controller
    {
        public int CountGames = 5;

        private readonly ICartService _cartService;
        private readonly IGameService _gameService;
        public CartsController(ICartService cartService, IGameService gameService)
        {
            _cartService = cartService;
            _gameService = gameService;
        }

        public async Task<IActionResult> Cart() => View();
    }
}
