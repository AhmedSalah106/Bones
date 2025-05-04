namespace Bones_App.DTOs
{
    public class PredictImageWithBodyPartDTO
    {
        public string BodyPart {  get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
