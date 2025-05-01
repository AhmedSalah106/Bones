using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bones_App.Models
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [ForeignKey("User")]
        public string UserId {  get; set; }
        public ApplicationUser? User { get; set; }
    }
}
