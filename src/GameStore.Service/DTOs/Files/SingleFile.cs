using Microsoft.AspNetCore.Http;

namespace GameStore.Service.DTOs.Files
{
    public class SingleFile
    {
        public IFormFile File { get; set; }
    }
}
