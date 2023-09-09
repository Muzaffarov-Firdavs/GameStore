using GameStore.Domain.Commons;

namespace GameStore.Domain.Entities.Games
{
    public class Genre : Auditable
    {
        public virtual Game Game { get; set; }
    }
}
