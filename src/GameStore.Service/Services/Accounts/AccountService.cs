using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Users;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.Commons.Security;
using GameStore.Service.DTOs.Accounts;
using GameStore.Service.Interfaces.Accounts;

namespace GameStore.Service.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IRepository<User> _repository;

        public AccountService(IMapper mapper, 
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IRepository<User> repository)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._authService = authService;
        }

        public async Task<bool> RegisterAsync(AccountRegisterDto dto)
        {
            var existAaccount = await _repository.SelectAsync(p =>
                p.Email.ToLower() == dto.Email.ToLower()
                || p.Username.ToLower() == dto.Username.ToLower()
                && !p.IsDeleted);
            if (existAaccount is not null)
            {
                if (existAaccount.Email.ToLower() == dto.Email.ToLower())
                    throw new CustomException(409, "This email is already registered.");

                else
                    throw new CustomException(409, "This username is already registered.");
            }

            var hashResult = PasswordHasher.Hash(dto.Password);
            var account = _mapper.Map<User>(dto);
            account.Password = hashResult.HashedPassword;
            account.Salt = hashResult.Salt;
            account.CreatedAt = DateTime.UtcNow;
            var result = await _repository.InsertAsync(account);
            await _unitOfWork.SaveAsync();
            return result != null;
        }

        public async Task<string> LoginAsync(AccountLoginDto accountLoginDto)
        {
            var user = await _repository.SelectAsync(p => !p.IsDeleted
                && p.Username.ToLower() == accountLoginDto.Username.ToLower(),
                includes: new string[] { "Image" });
            if (user is null)
                throw new CustomException(404, "No user with this username is found!");

            var verifyResult = PasswordHasher.Verify(accountLoginDto.Password, user.Salt, user.Password);
            if (!verifyResult)
                throw new CustomException(404, "Incorrect password!");
            
            string token = _authService.GenerateToken(user, user.Role);
            return token;
        }
    }
}
