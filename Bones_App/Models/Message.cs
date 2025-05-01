using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bones_App.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public string SenderId {  get; set; }
        public string ReceiverId {  get; set; }
        public DateTime SentAt { get; set; }
        [ForeignKey("SenderId")]
        public ApplicationUser? Sender { get; set; }
        [ForeignKey("ReceiverId")]
        public ApplicationUser? Receiver { get; set; }

    }
}
