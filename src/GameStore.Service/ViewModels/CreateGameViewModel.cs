using GameStore.Service.Commons.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.ViewModels
{
    public class CreateGameViewModel
    {
        [Required]
        public string Name { get; set; }
        [DescriptionCkeck]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public List<long> GenresIds { get; set; }

        [Required(ErrorMessage = "Please select an image.")]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
