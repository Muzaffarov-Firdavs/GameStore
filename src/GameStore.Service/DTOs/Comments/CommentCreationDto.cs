using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Comments
{
    public class CommentCreationDto
    {
        [MaxLength(600)]
        public string Text { get; set; }

        public long UserId { get; set; }

        public long GameId { get; set; }
    }
}
