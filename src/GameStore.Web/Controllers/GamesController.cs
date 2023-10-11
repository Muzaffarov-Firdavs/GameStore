using GameStore.Service.Commons.Extensions;
using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.Games;
using GameStore.Service.DTOs.SubComments;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using GameStore.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly ICommentService _commentService;

        public GamesController(IGameService gameService,
            IGenreService genreService,
            ICommentService commentService)
        {
            _gameService = gameService;
            _genreService = genreService;
            _commentService = commentService;
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

        [HttpPost]
        public async Task<IActionResult> AddComment(int gameId, string commentText)
        {
            var comment = new CommentCreationDto
            {
                Text = commentText,
                GameId = gameId,
                UserId = 1
            };

            await _commentService.AddAsync(comment);

            return RedirectToAction("Details", new { id = gameId });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int gameId, int commentId, string commentText)
        {
            var comment = new SubCommentCreationDto
            {
                Text = commentText,
                CommentId = commentId,
                UserId = 1
            };

            await _commentService.AddAsync(comment);

            return RedirectToAction("Details", new { id = gameId});
        }

    }
}
