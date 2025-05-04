using Azure;
using Bones_App.DTOs;
using Bones_App.Services.Interfaces;
using System.Net.Http;
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

            content.Add(fileContent, "image", imageFile.FileName); // Use correct key expected by model API

            var response = await httpClient.PostAsync("https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/single", content); // Update with actual URL
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ModelApiResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<ModelApiResponse> SendImageWithBodyPartToModelAsync(string BodyPart, IFormFile imageFile)
        {
            using var content = new MultipartFormDataContent();

            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);

            content.Add(fileContent, "image", imageFile.FileName);
            content.Add(new StringContent(BodyPart), "bodypart");

            var response = await httpClient.PostAsync("https://khaldoun52-final-models.hf.space/v1/api/medical-image-analysis/single/with-body-part", content); // Update with actual URL
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ModelApiResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        
    }
}
