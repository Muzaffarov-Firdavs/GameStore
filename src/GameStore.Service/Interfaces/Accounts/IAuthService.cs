using GameStore.Domain.Enums;
using GameStore.Service.DTOs.Accounts;

namespace GameStore.Service.Interfaces.Accounts
{
    public interface IAuthService
    {
        public string GenerateToken(AccountLoginDto human, Role role);
    }
}
