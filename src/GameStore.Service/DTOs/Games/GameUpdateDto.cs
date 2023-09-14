using GameStore.Service.DTOs.Genres;

namespace GameStore.Service.DTOs.Games
{
    public class GameUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<long> GenresIds { get; set; }
    }
}
