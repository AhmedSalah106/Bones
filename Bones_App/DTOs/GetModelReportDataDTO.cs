using System.Text.Json.Serialization;

namespace Bones_App.DTOs
{
    public class GetModelReportDataDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("received_time")]
        public DateTime ReceivedTime { get; set; }

        [JsonPropertyName("model_time_seconds")]
        public double ModelTimeSeconds { get; set; }

        [JsonPropertyName("body_part")]
        public string BodyPart { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("prediction")]
        public string Prediction { get; set; }

        [JsonPropertyName("fracture_confidence")]
        public double FractureConfidence { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
