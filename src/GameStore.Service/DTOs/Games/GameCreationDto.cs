namespace GameStore.Service.DTOs.Games
{
    public class GameCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<long> GenresIds { get; set; }

        //public long UserId { get; set; }
    }
}
