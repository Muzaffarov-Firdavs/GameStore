using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Users;

namespace GameStore.Domain.Entities.Orders
{
    public class Cart : Auditable
    {
        public decimal GrandTotalPrice { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}
