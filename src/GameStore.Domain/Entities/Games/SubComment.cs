using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities.Games
{
    public class SubComment : Auditable
    {
        [MaxLength(600)]
        public string Text { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
