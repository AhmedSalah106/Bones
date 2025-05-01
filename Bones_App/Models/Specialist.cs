using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bones_App.Models
{
    public class Specialist
    {

        public int Id { get; set; }
        [MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool IsVerified { get; set; }
        [Required]
        public string CertificateUrl {  get; set; }
        public bool IsDeleted {  get; set; } 

        [ForeignKey("User")]
        public string UserId {  get; set; }
        public ApplicationUser User { get; set; }
        public List<Patient>? Patients { get; set; }
    }
}
