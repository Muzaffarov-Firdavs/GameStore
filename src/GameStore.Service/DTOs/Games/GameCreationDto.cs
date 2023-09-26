using System.ComponentModel.DataAnnotations;

namespace GameStore.Service.DTOs.Games
{
    public class GameCreationDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<long> GenresIds { get; set; }

        //public long UserId { get; set; }
    }
}
