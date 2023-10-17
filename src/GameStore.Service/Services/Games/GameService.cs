using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Files;
using GameStore.Domain.Entities.Games;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Files;
using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Files;
using GameStore.Service.Interfaces.Games;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services.Games
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IRepository<Game> _repository;
        private readonly IRepository<Genre> _genreRepository;

        public GameService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IRepository<Game> repository,
            IRepository<Genre> genreRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _imageService = imageService;
            _genreRepository = genreRepository;
        }

        public async ValueTask<GameResultDto> AddAsync(GameCreationDto dto, ImageCreationDto imageDto)
        {
            // TODO: 3rd module requirement.
            //var user = await _userRepository.SelectAsync(u => u.Id == dto.UserId && !u.IsDeleted);
            //if (user == null)
            //    throw new CustomException(404, "User is not found");

            // Save image in static files
            Image image = null;
            if (imageDto is not null)
                image = await _imageService.UploadAsync(imageDto);

            var mappedGame = _mapper.Map<Game>(dto);
            mappedGame.ImageId = image is not null ? image.Id : null;
            mappedGame.CreatedAt = DateTime.UtcNow;

            // Connect genres with game
            if (dto.GenresIds != null)
                mappedGame.Genres = await _genreRepository.SelectAll(p =>
                    !p.IsDeleted && dto.GenresIds.Contains(p.Id)).ToListAsync();

            await _unitOfWork.CreateTransactionAsync();
            var result = await _repository.InsertAsync(mappedGame);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
            return _mapper.Map<GameResultDto>(result);
        }

        public async ValueTask<GameResultDto> ModifyAsync(long id, GameUpdateDto dto)
        {
            var game = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted,
                includes: new string[] { "Genres", "Comments", "Image" });
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            var mappedGame = _mapper.Map(dto, game);

            // Connect genres with exsisting game
            if (dto.GenresIds != null)
                mappedGame.Genres = await _genreRepository.SelectAll(p =>
                    !p.IsDeleted && dto.GenresIds.Contains(p.Id)).ToListAsync();

            mappedGame.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GameResultDto>(mappedGame);
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

        public async ValueTask<IEnumerable<GameResultDto>> RetrieveAllAsync(
                string search = null, long genreId = 0)
        {
            var games = await _repository.SelectAll(p => !p.IsDeleted)
                .Include(p => p.Genres)
                .Include(p => p.Image)
                .Include(p => p.Comments)
                .ToListAsync();

            if (string.IsNullOrWhiteSpace(search))
            {
                if (genreId != 0)
                {
                    Genre genre = await _genreRepository.SelectAsync(p => p.Id == genreId);
                    games = games.FindAll(p => p.Genres.Contains(genre));
                }
                return _mapper.Map<IEnumerable<GameResultDto>>(games);
            }

            games = games.FindAll(p => p.Name.ToLower().Contains(search.ToLower()));

            if (genreId != 0)
            {
                Genre genre = await _genreRepository.SelectAsync(p => p.Id == genreId);
                games = games.FindAll(p => p.Name.ToLower().Contains(search.ToLower()) 
                && p.Genres.Contains(genre));
            }

            return _mapper.Map<IEnumerable<GameResultDto>>(games);
        }

        public async ValueTask<Game> RetrieveByIdAsync(long id)
        {
            var game = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted,
                includes: new string[] { "Genres", "Comments.User.Image", "Image", "Comments.Comments.User.Image" });
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            return game;
        }

        public async ValueTask<IEnumerable<GameResultDto>> RetrieveAllByGenreAsync(long genreId)
        {
            var genre = await _genreRepository.SelectAsync(p => !p.IsDeleted && p.Id == genreId,
                new string[] { "Games", "Games.Image" });
            if (genre == null)
                throw new CustomException(404, "Genre is not found.");

            var results = genre.Games.Where(p => !p.IsDeleted);
            return _mapper.Map<IEnumerable<GameResultDto>>(results);
        }
    }
}
