using System.ComponentModel.DataAnnotations;

namespace Bones_App.DTOs
{
    public class AcceptSpecialistDTo
    {
        public int Id { get; set; }
        public IFormFile CertificateImage { get; set; }
    }
}
