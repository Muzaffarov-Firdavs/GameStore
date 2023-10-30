using GameStore.Service.DTOs.ContactInformations;
using GameStore.Service.Interfaces.Orders;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public ViewResult Order() => View("Order");

        [HttpPost]
        public async Task<IActionResult> Order(ContactInformationDto contactInfo)
        {
            if (!ModelState.IsValid)
                return Order();

            var info = await _orderService.AddContactInformationAsync(contactInfo);
            await _orderService.ConfirmOrderAsync(info);
            return View();
        }
    }
}
 