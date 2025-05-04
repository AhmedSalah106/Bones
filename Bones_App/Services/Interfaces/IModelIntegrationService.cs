using Bones_App.DTOs;

namespace Bones_App.Services.Interfaces
{
    public interface IModelIntegrationService
    {
        Task<ModelApiResponse> SendImageToModelAsync(IFormFile imageFile);
        Task<ModelApiResponse> SendImageWithBodyPartToModelAsync(string BodyPart, IFormFile imageFile);
        Task<GetModelReportResponseDTO> GetModelResponse(Guid Id);
    }
}
