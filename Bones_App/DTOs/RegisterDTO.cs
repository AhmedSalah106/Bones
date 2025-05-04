using System.ComponentModel.DataAnnotations;

namespace Bones_App.DTOs
{
    public class RegisterDTO
    {
        [Required,MinLength(2),MaxLength(50,ErrorMessage ="Maximum Length is 50")]
        public string FullName { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2"), MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword {  get; set; }

        [Required,EmailAddress]
        public string Email {  get; set; }
        
        [Required]
        public string PhoneNumber {  get; set; }

        [Required]
        public string Role { get; set; }  

        public IFormFile? CertificateFile{  get; set; }
        
        
    }
}
