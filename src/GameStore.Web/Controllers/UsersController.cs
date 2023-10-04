using GameStore.Service.Commons.Extensions;
using GameStore.Service.Commons.Helpers;
using GameStore.Service.DTOs.Files;
using GameStore.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> UploadAvatar()
        {
            long userId = (long)HttpContextHelper.UserId;
            ViewBag.User = await _userService.RetrieveByIdAsync(userId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(SingleFile imageDto)
        {
            var image = await imageDto.File.ToImageAsync();
            var result = await _userService.AddImageToProfileAsync(image);

            return RedirectToAction("uploadavatar", "users");
        }
    }
}
