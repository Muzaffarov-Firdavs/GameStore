using GameStore.Domain.Commons;

namespace GameStore.Domain.Entities.Games
{
    public class Genre : Auditable
    {
        public string Name { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
