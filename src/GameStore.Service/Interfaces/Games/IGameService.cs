using GameStore.Domain.Entities.Games;
using GameStore.Service.DTOs.Files;
using GameStore.Service.DTOs.Games;

namespace GameStore.Service.Interfaces.Games
{
    public interface IGameService
    {
        ValueTask<GameResultDto> AddAsync(GameCreationDto dto, ImageCreationDto imageDto);
        ValueTask<GameResultDto> ModifyAsync(long id, GameUpdateDto dto);
        ValueTask<bool> RemoveByIdAsync(long id);
        ValueTask<Game> RetrieveByIdAsync(long id);
        ValueTask<IEnumerable<GameResultDto>> RetrieveAllAsync(string search = null, long genreId = 0);
    }
}
