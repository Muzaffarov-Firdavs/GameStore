﻿using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Users;

namespace GameStore.Domain.Entities.Games
{
    public class Game : Auditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual User User { get; set; }
    }
}
