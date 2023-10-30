using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Entities.Users;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.Commons.Helpers;
using GameStore.Service.DTOs.ContactInformations;
using GameStore.Service.Interfaces.Orders;
using System.Text.RegularExpressions;

namespace GameStore.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ContactInformation> _contactInformationRepository;

        public OrderService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IRepository<Cart> cartRepository,
            IRepository<Order> orderRepository,
            IRepository<User> userRepository,
            IRepository<ContactInformation> contactInformationRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _contactInformationRepository = contactInformationRepository;
        }

        public async ValueTask<ContactInformation> AddContactInformationAsync(ContactInformationDto info)
        {
            var user = await _userRepository.SelectAsync(u =>
                !u.IsDeleted && u.Id == HttpContextHelper.UserId);
            if (user == null)

                throw new CustomException(401, "Anauthorised attempt.");
            if (!string.IsNullOrEmpty(info.Comment))
                info.Comment = Regex.Replace(info.Comment.Trim(), @"\s+", " ");

            var mappedInfo = _mapper.Map<ContactInformation>(info);
            mappedInfo.CreatedAt = DateTime.UtcNow;

            var result = await _contactInformationRepository.InsertAsync(mappedInfo);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async ValueTask<Order> ConfirmOrderAsync(ContactInformation contact)
        {
            var cart = await _cartRepository.SelectAsync(c => 
                !c.IsDeleted && c.UserId == HttpContextHelper.UserId,
                includes: new string[] { "Items.Game.Image"});
            cart.Items = cart.Items.Where(item => !item.IsDeleted && !item.Game.IsDeleted).ToList();
            cart.GrandTotalPrice = cart.Items.Sum(p => p.TotalPrice);
            if (!cart.Items.Any())
                throw new CustomException(404, "Cart items not found.");

            var order = new Order
            {
                GrandTotalPrice = cart.GrandTotalPrice,
                UserId = HttpContextHelper.UserId ??
                        throw new CustomException(401, "Anauthorised attempt."),
                User = await _userRepository.SelectAsync(u =>
                        !u.IsDeleted && u.Id == HttpContextHelper.UserId),
                ContactInformationId = contact.Id,
                ContactInformation = contact,
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            foreach(var cartItem in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    Id = cartItem.Id,
                    Amount = cartItem.Amount,
                    TotalPrice = cartItem.TotalPrice,
                    GameId = cartItem.GameId,
                    Game = cartItem.Game,
                    CreatedAt = cartItem.CreatedAt
                });
                cartItem.IsDeleted = true;
            }

            var result = await _orderRepository.InsertAsync(order);
            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
