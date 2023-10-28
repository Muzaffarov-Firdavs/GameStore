namespace GameStore.Service.DTOs.Carts
{
    public class CartResultDto
    {
        public long Id { get; set; }
        public decimal GrandTotalPrice { get; set; }
        public IEnumerable<CartItemResultDto> Items { get; set; }
    }
}
