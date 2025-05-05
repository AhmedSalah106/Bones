namespace Bones_App.DTOs
{
    public class UploadImageDTO
    {
        public int Id {  get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
