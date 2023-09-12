using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Games;

namespace GameStore.Service.Services.Games
{
    public class GenreService : IGenreService
    {
        public ValueTask<GenreResultDto> AddAsync(GenreCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<GenreResultDto> ModifyAsync(long id, GenreUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> RemoveByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<GenreResultDto>> RetrieveAllAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<GenreResultDto> RetrieveByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
