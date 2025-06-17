using Azure;
using Bones_App.DTOs;
using Bones_App.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Bones_App.Services.Implementation
{
    public class ModelIntegrationService : IModelIntegrationService
    {
        private readonly HttpClient httpClient;

        public ModelIntegrationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<GetModelReportResponseDTO> GetModelResponse(Guid Id)
        {
            var respone = await httpClient.GetAsync($"https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/reports/{Id}");
            if (!respone.IsSuccessStatusCode)
            {
                return new GetModelReportResponseDTO
                {
                    IsSuccess = false,
                    ErrorMessage = $"Model API returned status code {respone.StatusCode}"
                };
            }

            var JsonString = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var reportResponse = JsonSerializer.Deserialize<GetModelReportResponseDTO>(JsonString, options);

            return reportResponse;

        }

        public async Task<ModelApiResponse> SendImageToModelAsync(IFormFile imageFile)
        {
            using var content = new MultipartFormDataContent();

            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);

            content.Add(fileContent, "image", imageFile.FileName); 

            var response = await httpClient.PostAsync("https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/single", content); 
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ModelApiResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<ModelAPIsBatchResponseDTO> SendBatchofImages(List<IFormFile> imageFiles)
        {
            using var content = new MultipartFormDataContent();
            foreach (var file in imageFiles)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileContent = new ByteArrayContent(ms.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "images", file.FileName);
            }

            var response = await httpClient.PostAsync("https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/batch", content);
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ModelAPIsBatchResponseDTO>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }




        public async Task<ModelAPIsBatchResponseDTO> SendBatchOfImagesWithBodyPartAsync(string bodyParts, List<IFormFile> imageFiles)
        {
            using var content = new MultipartFormDataContent();

            foreach (var image in imageFiles)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                var fileContent = new ByteArrayContent(ms.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                content.Add(fileContent, "images", image.FileName); // key must be "images"
            }

            var requestUri = $"https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/batch/with-body-part?body_parts={Uri.EscapeDataString(bodyParts)}";

            var response = await httpClient.PostAsync(requestUri, content);
            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ModelAPIsBatchResponseDTO
                {
                    Is_Success = false,
                    Error_Message = $"API Error {response.StatusCode}: {jsonString}"
                };
            }

            try
            {
                return JsonSerializer.Deserialize<ModelAPIsBatchResponseDTO>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                return new ModelAPIsBatchResponseDTO
                {
                    Is_Success = false,
                    Error_Message = $"Deserialization failed: {ex.Message}"
                };
            }
        }


        public async Task<ModelApiResponse> SendImageWithBodyPartToModelAsync(string bodyPart, IFormFile imageFile)
        {
            using var content = new MultipartFormDataContent();
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileContent, "image", imageFile.FileName);

            var requestUri = $"https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/single/with-body-part?body_part={Uri.EscapeDataString(bodyPart)}";

            var response = await httpClient.PostAsync(requestUri, content);
            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ModelApiResponse
                {
                    Is_Success = false,
                    Error_Message = $"API Error {response.StatusCode}: {jsonString}"
                };
            }

            return JsonSerializer.Deserialize<ModelApiResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<ModelAPIsBatchResponseDTO> GetModelReport(UploadImageDTO imageDTO)
        {

            ModelAPIsBatchResponseDTO response;
            if (imageDTO.ImageFiles.Count == 0)
                return null;

            if (imageDTO.ImageFiles.Count == 1)
            {
                if (imageDTO.BodyPart == null)
                {
                    ModelApiResponse modelApiResponse = await SendImageToModelAsync(imageDTO.ImageFiles.First());
                    response = MapFromModelApiToModelBatchApi(modelApiResponse);
                }
                else
                {
                    ModelApiResponse modelApiResponse = await SendImageWithBodyPartToModelAsync(imageDTO.BodyPart, imageDTO.ImageFiles.First());
                    response = MapFromModelApiToModelBatchApi(modelApiResponse);
                }
            }
            else
            {
                if (imageDTO.BodyPart == null)
                    response = await SendBatchofImages(imageDTO.ImageFiles);
                else
                    response = await SendBatchOfImagesWithBodyPartAsync(imageDTO.BodyPart, imageDTO.ImageFiles);
            }

            return response;
        }

        public ModelAPIsBatchResponseDTO MapFromModelApiToModelBatchApi(ModelApiResponse singleResponse)
        {
            return new ModelAPIsBatchResponseDTO
            {
                Is_Success = singleResponse.Is_Success,
                Error_Message = singleResponse.Error_Message,
                Status_Code = singleResponse.Status_Code,
                Data = new List<ModelApiData> { singleResponse.Data }
            };
        }



    }
}
