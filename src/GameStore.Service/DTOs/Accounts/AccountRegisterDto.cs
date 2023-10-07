using GameStore.Service.Commons.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Accounts
{
    public class AccountRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [Email]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
