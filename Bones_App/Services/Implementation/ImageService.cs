using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class ImageService:Service<Image> , IImageService
    {
        private readonly IImageRepository imageRepository;
        public ImageService(IImageRepository imageRepository):base(imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public List<ImageResponseDTO> GetAllUserImagesDTO(int UserId)
        {
            List<ImageResponseDTO> AllImagesDTO
                = GetAll().Where(img=>img.PatientID==UserId).Select(img => new ImageResponseDTO 
                    { Id = img.Id, UploadedAt = img.UploadedAt, 
                        ImageUrl = img.ImageURL }).ToList();

            return AllImagesDTO;
        }

        public ImageResponseDTO GetImageResponseDTO(Image image)
        {
            ImageResponseDTO ImageDTO = new ImageResponseDTO()
            {
                Id = image.Id,
                UploadedAt = image.UploadedAt,
                ImageUrl = image.ImageURL
            };

            return ImageDTO;
        }
    }
}
