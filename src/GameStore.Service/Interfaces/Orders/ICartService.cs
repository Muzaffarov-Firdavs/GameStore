using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Entities.Users;
using GameStore.Service.DTOs.Carts;

namespace GameStore.Service.Interfaces.Orders
{
    public interface ICartService
    {
        ValueTask<Cart> AddCartAsync(User user);
        ValueTask<CartItemResultDto> AddItemAsync(long gameId);
        ValueTask<CartItemResultDto> SubtractItemAsync(long gameId);
        ValueTask<bool> RemoveItemAsync(long id);
        ValueTask<CartItemResultDto> RetrieveItemByIdAsync(long id);
        ValueTask<int> RetrieveCartItemsCountAsync();
        ValueTask<CartResultDto> RetrieveCartByUserIdAsync();
    }
}
