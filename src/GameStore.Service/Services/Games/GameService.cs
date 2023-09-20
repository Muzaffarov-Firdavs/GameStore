﻿using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Users;
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
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Genre> _genreRepository;

        public GameService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IRepository<Game> repository,
            IRepository<User> userRepository,
            IRepository<Genre> genreRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _imageService = imageService;
            _userRepository = userRepository;
            _genreRepository = genreRepository;
        }

        public async ValueTask<GameResultDto> AddAsync(GameCreationDto dto, ImageCreationDto imageDto)
        {
            var user = await _userRepository.SelectAsync(u => u.Id == dto.UserId && !u.IsDeleted);
            if (user == null)
                throw new CustomException(404, "User is not found");

            // Save image in static files
            var image = await _imageService.UploadAsync(imageDto);

            var mappedGame = _mapper.Map<Game>(dto);
            mappedGame.ImageId = image.Id;
            mappedGame.CreatedAt = DateTime.UtcNow;

            // Connect genres with exsisting game
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
                includes: new string[] {"Genres", "Comments", "Image"});
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            var mappedGame = _mapper.Map(dto, game);
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

        public async ValueTask<IEnumerable<GameResultDto>> RetrieveAllAsync(string search = null)
        {
            var games = await _repository.SelectAll(p => !p.IsDeleted)
                .Include(p => p.Genres)
                .Include(p => p.Comments)
                .Include(p => p.Image)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
                games = games.FindAll(p => p.Name.ToLower()
                .Contains(search.ToLower()));

            return _mapper.Map<IEnumerable<GameResultDto>>(games);
        }

        public async ValueTask<GameResultDto> RetrieveByIdAsync(long id)
        {
            var game = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted,
                includes: new string[] {"Genres", "Comments", "Image"});
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            return _mapper.Map<GameResultDto>(game);
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
