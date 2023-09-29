﻿using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Accounts
{
    public class AccountLoginDto
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
