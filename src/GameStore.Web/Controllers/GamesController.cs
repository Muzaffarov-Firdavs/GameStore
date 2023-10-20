using GameStore.Service.Commons.Exceptions;
using GameStore.Service.Commons.Extensions;
using GameStore.Service.Commons.Helpers;
using GameStore.Service.DTOs.Comments;
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
            var game = await _gameService.RetrieveByIdAsync(id);

            return View(new CommentGameViewModel { Game = game });
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
        public async Task<IActionResult> AddComment(CommentGameViewModel model)
        {
            try
            {
                model.Comment.UserId = (long)HttpContextHelper.UserId;
                model.Comment.GameId = model.Game.Id;
                await _commentService.AddAsync(model.Comment);
                return RedirectToAction("Details", new { id = model.Comment.GameId });
            }
            catch (CustomException ex)
            {
                var game = await _gameService.RetrieveByIdAsync(model.Comment.GameId);

                var viewModel = new CommentGameViewModel { Game = game };
                this.TempData["error"] = "Not saved - " + ex.Message;
                return View("Details", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(long commentId, long gameId, string text)
        {
            try
            {
                var dto = new CommentUpdateDto { Text = text };

                await _commentService.ModifyAsync(commentId, dto);

                return RedirectToAction("Details", new { id = gameId });
            }
            catch (CustomException ex)
            {
                var game = await _gameService.RetrieveByIdAsync(gameId);

                var viewModel = new CommentGameViewModel { Game = game };
                this.TempData["error"] = "Not edited - " + ex.Message;
                return View("Details", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(long commentId, long gameId)
        {
            await _commentService.RemoveByIdAsync(commentId);

            return RedirectToAction("Details", new { id = gameId });
        }

        [HttpPost]
        public async Task<IActionResult> RestoreComment(long commentId, long gameId)
        {
            await _commentService.RestoreByIdAsync(commentId);

            return RedirectToAction("Details", new { id = gameId });
        }
    }
}
