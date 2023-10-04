using GameStore.Service.Commons.Extensions;
using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create(CreateGameViewModel viewModel)
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

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Genres = await _genreService.RetrieveAllAsync();
            var result = await _gameService.RetrieveByIdAsync(id);
            var viewResult = new GameUpdateDto
            {
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                GenresIds = result.Genres.Select(x => x.Id).ToList()
            };
            return View(viewResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameUpdateDto dto)
        {
            await _gameService.ModifyAsync(id, dto);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewResult = await _gameService.RetrieveByIdAsync(id);
            return View(viewResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, GameResultDto dto)
        {
            await _gameService.RemoveByIdAsync(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
