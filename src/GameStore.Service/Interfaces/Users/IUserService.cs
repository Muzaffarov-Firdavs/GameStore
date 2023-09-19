using GameStore.Service.Commons.Configurations;
using GameStore.Service.DTOs.Users;

namespace GameStore.Service.Interfaces.Users
{
    public interface IUserService
    {
        ValueTask<UserResultDto> AddAsync(UserCreationDto dto);
        ValueTask<UserResultDto> ModifyAsync(long id, UserUpdateDto dto);
        ValueTask<bool> RemoveByIdAsync(long id);
        ValueTask<UserResultDto> RetrieveByIdAsync(long id);
        ValueTask<IEnumerable<UserResultDto>> RetrieveAllAsync(PaginationParams @params, string search = null);
    }
}
