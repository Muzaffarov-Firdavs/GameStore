using GameStore.Service.DTOs.Files;
using Microsoft.AspNetCore.Http;

namespace GameStore.Service.Commons.Extensions
{
    public static class FileExtension
    {
        public async static Task<ImageCreationDto> ToImageAsync(this IFormFile file)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }
            return new ImageCreationDto
            {
                File = bytes,
                FileName = file.FileName,
                FileExtension = Path.GetExtension(file.FileName)
            };
        }
    }
}
