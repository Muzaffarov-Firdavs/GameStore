using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Games;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services.Games
{
    public class GenreService : IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Genre> _repository;

        public GenreService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<Genre> repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async ValueTask<GenreResultDto> AddAsync(GenreCreationDto dto)
        {
            var genre = await _repository.SelectAsync(p => p.Name == dto.Name && !p.IsDeleted);
            if (genre != null)
                throw new CustomException(409, "Genre already exists.");

            var mappedGenre = _mapper.Map<Genre>(dto);
            mappedGenre.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.CreateTransactionAsync();
            var result = await _repository.InsertAsync(mappedGenre);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
            return _mapper.Map<GenreResultDto>(result);
        }

        public async ValueTask<GenreResultDto> ModifyAsync(long id, GenreUpdateDto dto)
        {
            var genre = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (genre == null)
                throw new CustomException(404, "Genre not found.");

            var mappedGenre = _mapper.Map(dto, genre);
            mappedGenre.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();

            return _mapper.Map<GenreResultDto>(mappedGenre);
        }

        public async ValueTask<bool> RemoveByIdAsync(long id)
        {
            var genre = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (genre == null)
                throw new CustomException(404, "Genre not found.");

            await _repository.DeleteAsync(genre);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async ValueTask<IEnumerable<GenreResultDto>> RetrieveAllAsync(string search = null)
        {
            var genres = await _repository.SelectAll(p => !p.IsDeleted).ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
                genres = genres.FindAll(p => p.Name.ToLower()
                .Contains(search.ToLower()));

            return _mapper.Map<IEnumerable<GenreResultDto>>(genres);
        }

        public async ValueTask<GenreResultDto> RetrieveByIdAsync(long id)
        {
            var genre = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (genre == null)
                throw new CustomException(404, "Genre not found.");

            return _mapper.Map<GenreResultDto>(genre);
        }
    }
}
