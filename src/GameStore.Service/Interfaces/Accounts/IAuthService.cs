using GameStore.Domain.Entities.Users;
using GameStore.Domain.Enums;

namespace GameStore.Service.Interfaces.Accounts
{
    public interface IAuthService
    {
        public string GenerateToken(User user, Role role);
    }
}
