using GameStore.Service.DTOs.Games;

namespace GameStore.Service.Interfaces.Games
{
    public interface IGameService
    {
        ValueTask<GameResultDto> AddAsync(GameCreationDto dto);
        ValueTask<GameResultDto> ModifyAsync(long id, GameUpdateDto dto);
        ValueTask<bool> RemoveByIdAsync(long id);
        ValueTask<GameResultDto> RetrieveByIdAsync(long id);
        ValueTask<IEnumerable<GameResultDto>> RetrieveAllAsync(string search = null);
        ValueTask<IEnumerable<GameResultDto>> RetrieveAllByGenreAsync(long genreId);
    }
}
