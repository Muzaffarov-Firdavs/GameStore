using GameStore.Domain.Commons;
using GameStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities.Orders
{
    public class ContactInformation : Auditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public PaymentType PaymentType { get; set; }

        [MaxLength(600)]
        public string Comment { get; set; }
    }
}
