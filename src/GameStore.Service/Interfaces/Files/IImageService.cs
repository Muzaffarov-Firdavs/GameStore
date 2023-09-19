
using GameStore.Domain.Entities.Files;
using GameStore.Service.DTOs.Files;

namespace GameStore.Service.Interfaces.Files
{
    public interface IImageService
    {
        ValueTask<Image> UploadAsync(ImageCreationDto dto);
        ValueTask<bool> DeleteAsync(long id);
    }
}
