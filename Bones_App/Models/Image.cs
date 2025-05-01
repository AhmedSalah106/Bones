using System.ComponentModel.DataAnnotations.Schema;

namespace Bones_App.Models
{
    public class Image
    {
        public int Id { get; set; }
        public DateTime UploadedAt { get; set; }
        public string ImageURL { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient? Patient { get; set; }

    }
}
