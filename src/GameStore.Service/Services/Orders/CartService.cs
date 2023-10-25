using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Entities.Users;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.Commons.Helpers;
using GameStore.Service.DTOs.Carts;
using GameStore.Service.Interfaces.Orders;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services.Orders
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<CartItem> _cartItemRepository;

        public CartService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IRepository<Cart> cartRepository,
            IRepository<Game> gameRepository,
            IRepository<CartItem> cartItemRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _gameRepository = gameRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async ValueTask<Cart> AddCartAsync(User user)
        {
            var cart = new Cart()
            {
                UserId = user.Id,
                User = user
            };

            var result = await _cartRepository.InsertAsync(cart);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async ValueTask<CartItemResultDto> AddItemAsync(CartItemCreationDto dto)
        {
            var game = await _gameRepository.SelectAsync(p => p.Id == dto.GameId && !p.IsDeleted);
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            var cart = await _cartRepository.SelectAsync(c =>
                c.UserId == HttpContextHelper.UserId && !c.IsDeleted);
            if (cart == null)
                throw new CustomException(404, "Cart is not found");

            var cartItem = new CartItem
            {
                //CartId = cart.Id,
                Cart = cart,
                Amount = dto.Amount,
                GameId = dto.GameId,
                TotalPrice = game.Price * dto.Amount,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _cartItemRepository.InsertAsync(cartItem);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CartItemResultDto>(result);
        }

        public async ValueTask<CartItemResultDto> ModifyItemAsync(CartItemUpdateDto dto)
        {
            var cartItem = await _cartItemRepository.SelectAsync(item =>    
                    !item.IsDeleted && item.Id == dto.Id,
                    includes: new string[] { "Game" });
            if (cartItem == null)
                throw new CustomException(404, "Cart item is not found.");

            cartItem.Amount = dto.Amount;
            cartItem.TotalPrice = cartItem.Game.Price * dto.Amount;
            cartItem.UpdatedAt = DateTime.UtcNow;
            var result = _cartItemRepository.UpdateAsync(cartItem);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CartItemResultDto>(result);

        }

        public async ValueTask<bool> RemoveItemAsync(long id)
        {
            var cartItem = await _cartItemRepository.SelectAsync(item =>
                    !item.IsDeleted && item.Id == id);
            if (cartItem == null)
                throw new CustomException(404, "Cart item is not found.");

            await _cartItemRepository.DeleteAsync(cartItem);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async ValueTask<IEnumerable<CartItemResultDto>> RetrieveAllAsync()
        {
            var items = await _cartItemRepository.SelectAll(item => !item.IsDeleted,
                includes: new string[] { "Game" })
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartItemResultDto>>(items);
        }

        public async ValueTask<CartResultDto> RetrieveCartByUserIdAsync()
        {
            var cart = await _cartRepository.SelectAsync(c =>
                !c.IsDeleted && c.UserId == HttpContextHelper.UserId,
                includes: new string[] { "Items" });
            if (cart == null)
                throw new CustomException(404, "Cart is not found.");

            return _mapper.Map<CartResultDto>(cart);
        }

        public async ValueTask<CartItemResultDto> RetrieveItemByIdAsync(long id)
        {
            var cartItem = await _cartItemRepository.SelectAsync(item =>
                !item.IsDeleted && item.Id == id,
                includes: new string[] { "Game" });
            if (cartItem == null)
                throw new CustomException(404, "Cart item is not found.");

            return _mapper.Map<CartItemResultDto>(cartItem);
        }
    }
}
