using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Files;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Users;
using GameStore.Service.Commons.Configurations;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.Commons.Extensions;
using GameStore.Service.Commons.Helpers;
using GameStore.Service.DTOs.Files;
using GameStore.Service.DTOs.Users;
using GameStore.Service.Interfaces.Files;
using GameStore.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IRepository<User> _repository;

        public UserService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IRepository<User> repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _imageService = imageService;
        }

        public async ValueTask<UserResultDto> AddAsync(UserCreationDto dto)
        {
            var user = await _repository
                .SelectAsync(p => p.Email.ToLower() == dto.Email.ToLower()
                || p.Username.ToLower() == dto.Username.ToLower()
                && !p.IsDeleted);
            if (user != null)
                throw new CustomException(409, "User already exists.");

            var mappedUser = _mapper.Map<User>(dto);
            mappedUser.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.CreateTransactionAsync();
            var result = await _repository.InsertAsync(mappedUser);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserResultDto>(result);
        }

        public async ValueTask<UserResultDto> ModifyAsync(long id, UserUpdateDto dto)
        {
            var user = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (user == null)
                throw new CustomException(404, "User is not found.");

            var mappedUser = _mapper.Map(dto, user);
            mappedUser.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UserResultDto>(mappedUser);
        }

        public async ValueTask<bool> RemoveByIdAsync(long id)
        {
            var user = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (user == null)
                throw new CustomException(404, "User is not found.");

            await _repository.DeleteAsync(user);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync(PaginationParams @params, string search = null)
        {
            var users = await _repository.SelectAll(p => !p.IsDeleted)
                .ToPagedList(@params)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
                users = users.FindAll(p =>
                p.FirstName.ToLower().Contains(search.ToLower())
                || p.LastName.ToLower().Contains(search.ToLower())
                || p.Username.ToLower().Contains(search.ToLower()));

            return _mapper.Map<IEnumerable<UserResultDto>>(users);
        }

        public async ValueTask<UserResultDto> RetrieveByIdAsync(long id)
        {
            var user = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted,
                includes: new string[] { "Image" });
            if (user == null)
                throw new CustomException(404, "User is not found.");

            return _mapper.Map<UserResultDto>(user);
        }

        public async ValueTask<UserResultDto> AddImageToProfileAsync(ImageCreationDto imageDto)
        {
            var user = await _repository.SelectAsync(p => p.Id == HttpContextHelper.UserId && !p.IsDeleted,
                includes: new string[] { "Image" });
            if (user == null)
                throw new CustomException(404, "User is not found.");

            Image image = null;
            if (imageDto is not null)
                image = await _imageService.UploadAsync(imageDto);

            user.ImageId = image is not null ? image.Id : null;
            user.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UserResultDto>(user);
        }
    }
}
