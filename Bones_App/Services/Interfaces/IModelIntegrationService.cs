using Bones_App.DTOs;

namespace Bones_App.Services.Interfaces
{
    public interface IModelIntegrationService
    {
        Task<ModelApiResponse> SendImageToModelAsync(IFormFile imageFile);
        Task<ModelApiResponse> SendImageWithBodyPartToModelAsync(string BodyPart, IFormFile imageFile);
        Task<ModelAPIsBatchResponseDTO> SendBatchOfImagesWithBodyPartAsync(string BodyPart, List<IFormFile> imageFiles);
        Task<ModelAPIsBatchResponseDTO> SendBatchofImages(List<IFormFile> imageFiles);
        Task<ModelAPIsBatchResponseDTO> GetModelReport(UploadImageDTO imageDTO);
        Task<GetModelReportResponseDTO> GetModelResponse(Guid Id);
        ModelAPIsBatchResponseDTO MapFromModelApiToModelBatchApi(ModelApiResponse modelApiResponse);
    }
}
