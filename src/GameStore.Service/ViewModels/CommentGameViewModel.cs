using GameStore.Domain.Entities.Games;

namespace GameStore.Service.ViewModels
{
    public class CommentGameViewModel
    {
        public Game Game { get; set; }
        public Comment Comment { get; set; }
    }
}
