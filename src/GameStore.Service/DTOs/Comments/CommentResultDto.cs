using GameStore.Service.DTOs.Users;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Comments
{
    public class CommentResultDto
    {
        public long Id { get; set; }
        [MaxLength(600)]
        public string Text { get; set; }

        public UserResultDto User { get; set; }

        public CommentResultDto Parent { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<CommentResultDto> Comments { get; set; }
    }
}
