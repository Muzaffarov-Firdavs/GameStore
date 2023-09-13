using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Games;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class GenresController : BaseController
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GenreCreationDto dto)
            => Ok(await _genreService.AddAsync(dto));

        [HttpPut("id")]
        public async Task<IActionResult> PutAsync(long id, GenreUpdateDto dto)
            => Ok(await _genreService.ModifyAsync(id, dto));

        [HttpDelete("id")]
        public async Task<IActionResult> DaleteByIdAsync(long id)
            => Ok(await _genreService.RemoveByIdAsync(id));

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(await _genreService.RetrieveByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search = null)
            => Ok(await _genreService.RetrieveAllAsync(search));

    }
}
