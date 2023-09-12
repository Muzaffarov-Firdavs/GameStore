using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Users;

namespace GameStore.Service.DTOs.Games
{
    public class GameCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public long UserId { get; set; }
    }
}
