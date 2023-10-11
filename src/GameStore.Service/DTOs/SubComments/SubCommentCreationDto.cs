using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.SubComments
{
    public class SubCommentCreationDto
    {
        [MaxLength(600)]
        [Required]
        public string Text { get; set; }

        public long UserId { get; set; }

        public long CommentId { get; set; }
    }
}
