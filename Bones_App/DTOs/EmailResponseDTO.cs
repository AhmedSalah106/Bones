namespace Bones_App.DTOs
{
    public class EmailResponseDTO
    {
       public DateTime DateSent {  get; set; }
       public string Body {  get; set; }
       public string From { get; set; }
       public string Subject {  get; set; }
       public string UserID {  get; set; }
    }
}
