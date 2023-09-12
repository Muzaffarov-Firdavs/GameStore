using GameStore.Service.DTOs.Genres;

namespace GameStore.Service.Interfaces.Games
{
    public interface IGenreService
    {
        ValueTask<GenreResultDto> AddAsync(GenreCreationDto dto);
        ValueTask<GenreResultDto> ModifyAsync(long id, GenreUpdateDto dto);
        ValueTask<bool> RemoveByIdAsync(long id);
        ValueTask<GenreResultDto> RetrieveByIdAsync(long id);
        ValueTask<IEnumerable<GenreResultDto>> RetrieveAllAsync(string search = null);
    }
}
