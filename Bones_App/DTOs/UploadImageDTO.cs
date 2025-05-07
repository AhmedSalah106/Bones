namespace Bones_App.DTOs
{
    public class UploadImageDTO
    {
        public int Id { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public DateTime UploadedAt { get; set; }
        public string? BodyPart { get; set; }

    }
}
