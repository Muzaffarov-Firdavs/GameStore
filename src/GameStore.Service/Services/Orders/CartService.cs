using GameStore.Service.DTOs.Carts;
using GameStore.Service.Interfaces.Orders;

namespace GameStore.Service.Services.Orders
{
    public class CartService : ICartService
    {
        public ValueTask<CartItemResultDto> AddItemAsync(CartItemCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CartItemResultDto> ModifyItemAsync(CartItemUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> RemoveItemAsync(long id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<CartItemResultDto>> RetrieveAllAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<CartResultDto> RetrieveByClientIdAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<CartItemResultDto> RetrieveByItemIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
