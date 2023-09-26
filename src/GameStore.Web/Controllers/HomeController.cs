using GameStore.Service.Interfaces.Games;
using GameStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace GameStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService _gameService;

        public HomeController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index([MinLength(3)]string search)
        {
            var games = await _gameService.RetrieveAllAsync(search);
            return View(games);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}