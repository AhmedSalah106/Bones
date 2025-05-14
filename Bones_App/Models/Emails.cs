using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bones_App.Models
{
    public class Emails
    {
        public int id {  get; set; }
        [Required]
        public string From {  get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]  
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
        [ForeignKey("Specialist")]
        public string UserId {  get; set; }
    }
}
