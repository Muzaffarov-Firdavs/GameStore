using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Files;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.Commons.Helpers;
using GameStore.Service.DTOs.Files;
using GameStore.Service.Interfaces.Files;
using Path = System.IO.Path;

namespace GameStore.Service.Services.Files
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Image> _repository;
        public ImageService(IUnitOfWork unitOfWork, IRepository<Image> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async ValueTask<bool> RemoveAsync(long id)
        {
            var image = await _repository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (image == null)
                throw new CustomException(404, "Image is not found");

            await this._repository.DeleteAsync(image);
            await this._unitOfWork.SaveAsync();
            
            return true;
        }

        public async ValueTask<Image> UploadAsync(ImageCreationDto dto)
        {
            string path = Path.Combine(EnvironmentHelper.WebRootPath, "Images");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = $"{Guid.NewGuid()}{dto.FileExtension}";
            string fullPath = Path.Combine(path, fileName);

            // creating file in created or existed folder and write all content
            FileStream targetFile = new FileStream(fullPath, FileMode.OpenOrCreate);
            await targetFile.WriteAsync(dto.File);

            Image attachment = new Image
            {
                FileName = fileName,
                FilePath = fullPath,
                CreatedAt = DateTime.UtcNow,
            };
            var insertedFile = await this._repository.InsertAsync(attachment);
            await _unitOfWork.SaveAsync();

            return insertedFile;
        }
    }
}
