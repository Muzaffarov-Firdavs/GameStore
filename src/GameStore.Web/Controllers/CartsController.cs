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

        public async Task<IActionResult> Cart()
        {
            var cart = await _cartService.RetrieveCartByUserIdAsync();
            return View(cart);
        }

        public async Task<IActionResult> AddItem(long gameId)
        {
            await _cartService.AddItemAsync(gameId);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SubtractItem(long gameId)
        {
            await _cartService.SubtractItemAsync(gameId);
            return RedirectToAction("Cart", "Carts");
        }

        public async Task<IActionResult> DeleteItem(long itemId)
        {
            await _cartService.RemoveItemAsync(itemId);
            return RedirectToAction("Cart", "Carts");
        }
    }
}
