using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Entities.Users;
using GameStore.Service.DTOs.Carts;

namespace GameStore.Service.Interfaces.Orders
{
    public interface ICartService
    {
        ValueTask<Cart> AddCartAsync(User user);
        ValueTask<CartItemResultDto> AddItemAsync(CartItemCreationDto dto);
        ValueTask<CartItemResultDto> ModifyItemAsync(CartItemUpdateDto dto);
        ValueTask<bool> RemoveItemAsync(long id);
        ValueTask<CartItemResultDto> RetrieveItemByIdAsync(long id);
        ValueTask<IEnumerable<CartItemResultDto>> RetrieveAllAsync();
        ValueTask<CartResultDto> RetrieveCartByUserIdAsync();
    }
}
