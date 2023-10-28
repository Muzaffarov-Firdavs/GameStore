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

        public async ValueTask<CartItemResultDto> AddItemAsync(long gameId)
        {
            var game = await _gameRepository.SelectAsync(p => p.Id == gameId && !p.IsDeleted);
            if (game == null)
                throw new CustomException(404, "Game is not found.");

            var cart = await _cartRepository.SelectAsync(c =>
                c.UserId == HttpContextHelper.UserId && !c.IsDeleted);
            if (cart == null)
                throw new CustomException(404, "Cart is not found");

            var cartItem = await _cartItemRepository.SelectAsync(p => p.GameId == gameId && !p.IsDeleted);
            if (cartItem == null)
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    Cart = cart,
                    GameId = gameId,
                    Game = game,
                    Amount = 1,
                    TotalPrice = game.Price,
                    CreatedAt = DateTime.UtcNow,
                };

                var result = await _cartItemRepository.InsertAsync(newCartItem);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<CartItemResultDto>(result);
            }
            else
            {
                cartItem.Amount += 1;
                cartItem.TotalPrice += game.Price;
                cartItem.UpdatedAt = DateTime.UtcNow;

                var result = await _cartItemRepository.UpdateAsync(cartItem);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<CartItemResultDto>(result);
            }
        }

        public async ValueTask<CartItemResultDto> SubtractItemAsync(long gameId)
        {
            var cartItem = await _cartItemRepository.SelectAsync(item =>    
                    !item.IsDeleted && item.GameId == gameId,
                    includes: new string[] { "Game" });
            if (cartItem == null)
                throw new CustomException(404, "Cart item is not found.");

            if (cartItem.Amount > 1)
            {
                cartItem.Amount -= 1;
                cartItem.TotalPrice -= cartItem.Game.Price;
                cartItem.UpdatedAt = DateTime.UtcNow;

                var result = await _cartItemRepository.UpdateAsync(cartItem);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<CartItemResultDto>(result); 
            }
            else
            {
                await _cartItemRepository.DeleteAsync(cartItem);
                await _unitOfWork.SaveAsync();
                return null;
            }
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

        public async ValueTask<int> RetrieveCartItemsCountAsync()
        {
            var cart = await _cartRepository.SelectAsync(c =>
                !c.IsDeleted && c.UserId == HttpContextHelper.UserId,
                includes: new string[] { "Items.Game" });
            if (cart == null)
                throw new CustomException(404, "Cart is not found.");

            cart.Items = cart.Items.Where(item => !item.IsDeleted && !item.Game.IsDeleted).ToList();
            return cart.Items.Sum(p => p.Amount);
        }

        public async ValueTask<CartResultDto> RetrieveCartByUserIdAsync()
        {
            var cart = await _cartRepository.SelectAsync(c =>
                !c.IsDeleted && c.UserId == HttpContextHelper.UserId,
                includes: new string[] { "Items.Game.Image" });
            if (cart == null)
                throw new CustomException(404, "Cart is not found.");

            cart.Items = cart.Items.Where(item => !item.IsDeleted && !item.Game.IsDeleted).ToList();
            cart.GrandTotalPrice = cart.Items.Sum(p => p.TotalPrice);

            return _mapper.Map<CartResultDto>(cart);
        }
    }
}
