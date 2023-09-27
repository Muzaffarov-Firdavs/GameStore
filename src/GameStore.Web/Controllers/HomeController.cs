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
        private readonly IGenreService _genreService;

        public HomeController(IGameService gameService, IGenreService genreService)
        {
            _gameService = gameService;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index([MinLength(3)]string search, long genreId = 0)
        {
            ViewBag.Genres = await _genreService.RetrieveAllAsync();

            if (genreId != 0)
            {
                var genreGames = await _gameService.RetrieveAllByGenreAsync(genreId);
                return View(genreGames);
            }

            var games = await _gameService.RetrieveAllAsync(search);
            return View(games);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}