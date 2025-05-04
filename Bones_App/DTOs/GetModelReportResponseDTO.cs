namespace Bones_App.DTOs
{
    public class GetModelReportResponseDTO
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public string StatusCode { get; set; }
        public GetModelReportDataDTO Data { get; set; }
    }
}
