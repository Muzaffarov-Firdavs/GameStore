using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Games;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GameCreationDto dto)
            => Ok(await _gameService.AddAsync(dto));

        [HttpPut("id")]
        public async Task<IActionResult> PutAsync(long id, GameUpdateDto dto)
            => Ok(await _gameService.ModifyAsync(id, dto));

        [HttpDelete("id")]
        public async Task<IActionResult> DaleteByIdAsync(long id)
            => Ok(await _gameService.RemoveByIdAsync(id));

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(await _gameService.RetrieveByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search = null)
            => Ok(await _gameService.RetrieveAllAsync(search));

    }

}
