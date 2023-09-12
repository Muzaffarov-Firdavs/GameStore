using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Games;

namespace GameStore.Service.Services.Games
{
    public class GameService : IGameService
    {
        public ValueTask<GameResultDto> AddAsync(GameCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<GameResultDto> ModifyAsync(long id, GameUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> RemoveByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<GameResultDto>> RetrieveAllAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<GameResultDto> RetrieveByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
