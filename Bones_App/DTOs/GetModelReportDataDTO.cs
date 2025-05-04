namespace Bones_App.DTOs
{
    public class GetModelReportDataDTO
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Filename { get; set; }
        public string Format { get; set; }
        public DateTime ReceivedTime { get; set; }
        public double ModelTimeSeconds { get; set; }
        public string BodyPart { get; set; }
        public double Confidence { get; set; }
        public string Prediction { get; set; }
        public double FractureConfidence { get; set; }
        public string Error { get; set; }
    }
}
