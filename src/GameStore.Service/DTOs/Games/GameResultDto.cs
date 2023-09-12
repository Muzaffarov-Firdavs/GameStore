using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.Genres;

namespace GameStore.Service.DTOs.Games
{
    public class GameResultDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<GenreResultDto> Genres { get; set; }
        public virtual ICollection<CommentResultDto> Comments { get; set; }
    }
}
