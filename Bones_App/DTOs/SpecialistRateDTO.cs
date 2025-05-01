using System.ComponentModel.DataAnnotations;

namespace Bones_App.DTOs
{
    public class SpecialistRateDTO
    {
        public int PatientId {  get; set; }
        public int SpecialistId {  get; set; }
        [Range(1, 5)]
        public int RatingValue {  get; set; }
        public string? Comment {  get; set; }
        public DateTime RatedAt { get; set; }
    }
}
