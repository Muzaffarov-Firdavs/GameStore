using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.ViewModels
{
    public class CreateGameViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }
        public List<long> GenresIds { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
