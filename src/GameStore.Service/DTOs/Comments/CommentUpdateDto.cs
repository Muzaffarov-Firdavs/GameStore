using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Comments
{
    public class CommentUpdateDto
    {
        [MaxLength(600)]
        public string Text { get; set; }
    }
}
