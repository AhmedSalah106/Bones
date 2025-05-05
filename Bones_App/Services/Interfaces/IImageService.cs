using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IImageService:IService<Image> 
    {
        List<ImageResponseDTO> GetAllUserImagesDTO(string Id);

        ImageResponseDTO GetImageResponseDTO(Image image);
        Response<List<ImageResponseDTO>> RetrieveImage(string Id);
    }
}
