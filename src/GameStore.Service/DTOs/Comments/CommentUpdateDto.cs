using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Comments
{
    public class CommentUpdateDto
    {
        public long Id { get; set; }

        [MaxLength(600)]
        public string Text { get; set; }
    }
}
