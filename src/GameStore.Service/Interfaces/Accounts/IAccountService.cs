using GameStore.Service.DTOs.Accounts;

namespace GameStore.Service.Interfaces.Accounts
{
    public interface IAccountService
    {
        public Task<bool> RegisterAsync(AccountRegisterDto dto);
        public Task<string> LoginAsync(AccountLoginDto accountLoginDto);
    }
}
