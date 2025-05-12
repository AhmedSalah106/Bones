namespace Bones_App.DTOs
{
    public class GetChatResponseDTO
    {
        public string Content { get; set; }
        public string ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderId {  get; set; }
    }
}
