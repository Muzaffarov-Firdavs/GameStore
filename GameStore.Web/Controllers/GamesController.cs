using GameStore.Service.Commons.Extensions;
using GameStore.Service.DTOs.Files;
using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GameStore.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;

        public GamesController(IGameService gameService, IGenreService genreService)
        {
            _gameService = gameService;
            _genreService = genreService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _gameService.RetrieveByIdAsync(id));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _genreService.RetrieveAllAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel viewModel/*GameCreationDto dto, ImageCreationDto? image = null*/)
        {
            var dto = new GameCreationDto
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Description = viewModel.Description,
                GenresIds = viewModel.GenresIds
            };
            var image = await viewModel.ImageFile.ToImageAsync();

            await _gameService.AddAsync(dto, image);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
