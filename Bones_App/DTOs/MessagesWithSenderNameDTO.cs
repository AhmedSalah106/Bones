namespace Bones_App.DTOs
{
    public class MessagesWithSenderNameDTO
    {
        public string SenderName { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
       
    }
}
