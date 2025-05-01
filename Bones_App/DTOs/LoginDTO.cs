using System.ComponentModel.DataAnnotations;

namespace Bones_App.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email {  get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
