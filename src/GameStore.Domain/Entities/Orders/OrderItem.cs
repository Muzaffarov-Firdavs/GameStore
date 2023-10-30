using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Games;

namespace GameStore.Domain.Entities.Orders
{
    public class OrderItem : Auditable
    {
        public long GameId { get; set; }
        public Game Game { get; set; }

        public int Amount { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual long OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
