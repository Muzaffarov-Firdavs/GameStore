using GameStore.Service.DTOs.Users;

namespace GameStore.Service.DTOs.SubComments
{
    public class SubCommentResultDto
    {
        public long Id { get; set; }
        public string Text { get; set; }

        public UserResultDto User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
