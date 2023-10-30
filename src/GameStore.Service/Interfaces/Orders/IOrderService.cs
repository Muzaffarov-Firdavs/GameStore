using GameStore.Domain.Entities.Orders;
using GameStore.Service.DTOs.ContactInformations;

namespace GameStore.Service.Interfaces.Orders
{
    public interface IOrderService
    {
        ValueTask<ContactInformation> AddContactInformationAsync(ContactInformationDto info);
        ValueTask<Order> ConfirmOrderAsync(ContactInformation contact);
    }
}
