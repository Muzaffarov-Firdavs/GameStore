using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Games;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services.Games
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Game> _repository;

        public GameService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<Game> repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async ValueTask<GameResultDto> AddAsync(GameCreationDto dto)
        {
            var mappedGame = _mapper.Map<Game>(dto);
            mappedGame.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.CreateTransactionAsync();
            var result = _repository.InsertAsync(mappedGame);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
            return _mapper.Map<GameResultDto>(result);
        }

        public async ValueTask<GameResultDto> ModifyAsync(long id, GameUpdateDto dto)
        {
            var game = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            var mappedGame = _mapper.Map(dto, game);
            mappedGame.UpdatedAt = DateTime.UtcNow;

            var result = _repository.UpdateAsync(mappedGame);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GameResultDto>(result);
        }

        public async ValueTask<bool> RemoveByIdAsync(long id)
        {
            var game = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            await _repository.DeleteAsync(game);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async ValueTask<IEnumerable<GameResultDto>> RetrieveAllAsync(string search = null)
        {
            var games = await _repository
                .SelectAll(p => p.Name.Contains(search) && !p.IsDeleted)
                .ToListAsync();

            IEnumerable<GameResultDto> result = new List<GameResultDto>();
            return _mapper.Map(games, result);
        }

        public async ValueTask<GameResultDto> RetrieveByIdAsync(long id)
        {
            var game = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            return _mapper.Map<GameResultDto>(game);
        }
    }
}
