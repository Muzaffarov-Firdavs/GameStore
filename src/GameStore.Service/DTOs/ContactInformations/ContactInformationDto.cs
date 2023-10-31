using GameStore.Domain.Enums;
using GameStore.Service.Commons.Attributes;
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
        [Email]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone number.")]
        public string Phone { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }

        public string Comment { get; set; }
    }
}
