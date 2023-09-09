﻿using GameStore.Domain.Commons;
using GameStore.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities.Games
{
    public class Comment : Auditable
    {
        [MaxLength(600)]
        public string Text { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public virtual Game Game { get; set; }
    }
}
