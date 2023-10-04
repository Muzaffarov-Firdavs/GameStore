using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Files;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Enums;

namespace GameStore.Domain.Entities.Users
{
    public class User : Auditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public Role Role { get; set; } = Role.User;

        public long? ImageId { get; set; }
        public virtual Image Image { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
