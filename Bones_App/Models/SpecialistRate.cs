using System.ComponentModel.DataAnnotations;

namespace Bones_App.Models
{
    public class SpecialistRate
    {
        public int Id { get; set; }
        [Required]
        public int PatientId {  get; set; }
        [Required]
        public int SpecialistId { get; set; }
        [Range(1, 5)]
        public int RatingValue {  get; set; }
        public DateTime RatedAt { get; set; }
        public string? Comment { get; set; }
    }
}
