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

        public async Task<ActionResult<int>> RetrieveItemCount()
        {
            return await _cartService.RetrieveCartItemsCountAsync();
        }

        public async Task<IActionResult> Cart()
        {
            var cart = await _cartService.RetrieveCartByUserIdAsync();
            return View(cart);
        }

        public async Task<IActionResult> AddItem(long gameId, string? returnUrl = null)
        {
            await _cartService.AddItemAsync(gameId);

            if (returnUrl != null)
                return Redirect(returnUrl);

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
