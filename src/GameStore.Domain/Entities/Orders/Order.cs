using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Users;

namespace GameStore.Domain.Entities.Orders
{
    public class Order : Auditable
    {
        public decimal GrandTotalPrice { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

        public long ContactInformationId {  get; set; }
        public ContactInformation ContactInformation { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
