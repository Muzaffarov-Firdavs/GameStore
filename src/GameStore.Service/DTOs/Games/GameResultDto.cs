using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.Files;
using GameStore.Service.DTOs.Genres;

namespace GameStore.Service.DTOs.Games
{
    public class GameResultDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ImageResultDto Image { get; set; }
        public List<GenreResultDto> Genres { get; set; }
        public List<CommentResultDto> Comments { get; set; }
    }
}
