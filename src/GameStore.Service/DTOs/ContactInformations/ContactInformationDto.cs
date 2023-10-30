using GameStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.ContactInformations
{
    public class ContactInformationDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }

        [MaxLength(600)]
        public string Comment { get; set; }
    }
}
