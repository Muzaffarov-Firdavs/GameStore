using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.SubComments
{
    public class SubCommentUpdateDto
    {
        [MaxLength(600)]
        [Required]
        public string Text { get; set; }
    }
}
