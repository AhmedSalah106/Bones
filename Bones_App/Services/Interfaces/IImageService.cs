using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IImageService:IService<Image> 
    {
        List<ImageResponseDTO> GetAllUserImagesDTO(int UserId);

        ImageResponseDTO GetImageResponseDTO(Image image);
    }
}
