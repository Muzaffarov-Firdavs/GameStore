using GameStore.Service.DTOs.Accounts;
using GameStore.Service.Interfaces.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("register")]
        public ViewResult Register() => View("Register");

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AccountRegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                bool result = await _accountService.RegisterAsync(dto);
                if (result)
                    return RedirectToAction("login", "accounts");
            }

            return Register();
        }

        [HttpGet("login")]
        public ViewResult Login() => View("Login");

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AccountLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var token = await _accountService.LoginAsync(dto);
                HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
                return RedirectToAction("Index", "Home");
            }

            return Login();
        }
    }
}
