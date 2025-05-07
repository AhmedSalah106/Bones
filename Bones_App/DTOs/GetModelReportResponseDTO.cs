using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class GetModelReportResponseDTO
    {
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("is_success")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("status_code")]
        public string StatusCode { get; set; }

        [JsonPropertyName("data")]
        public GetModelReportDataDTO Data { get; set; }
    }
}
