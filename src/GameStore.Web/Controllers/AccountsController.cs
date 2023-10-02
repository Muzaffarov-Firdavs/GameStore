using GameStore.Service.DTOs.Accounts;
using GameStore.Service.Interfaces.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [Route("Accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("Register")]
        public ViewResult Register() => View("Register");

        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountRegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                bool result = await _accountService.RegisterAsync(dto);
                if (result)
                    return RedirectToAction("login", "accounts");
            }

            return Register();
        }

        [HttpGet]
        [Route("Login")]
        public ViewResult Login() => View("Login");

        [HttpPost("login")]
        public async Task<IActionResult> Login(AccountLoginDto dto)
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

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1)
            });
            return RedirectToAction("Index", "Home");
        }
    }
}
