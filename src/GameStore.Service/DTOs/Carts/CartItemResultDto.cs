using GameStore.Service.DTOs.Games;

namespace GameStore.Service.DTOs.Carts
{
    public class CartItemResultDto
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public GameResultDto Game { get; set; }
        public int Amount { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsOrdered { get; set; }
    }
}
