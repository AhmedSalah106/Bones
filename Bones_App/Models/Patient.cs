using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bones_App.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [MinLength(3) , MaxLength(50)]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage ="Enter Valid E-mail")]
        public string? Email { get; set; }
        [ForeignKey("User")]
        public string UserId {  get; set; }
        public ApplicationUser User { get; set; }
        public bool IsPaidUser { get; set; }
        public int FreeLimit { get; set; }
        public bool IsDeletedUser {  get; set; } 
        public List<Image>? Images { get; set; } 
        public List<Specialist>? Specialists { get; set; }
    }
}
