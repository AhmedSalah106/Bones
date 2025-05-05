namespace Bones_App.DTOs
{
    public class JWTResponseDTO
    {
        public string Token {  get; set; }
        public DateTime Expire { get; set; }
        public string UserId {  get; set; }
        public int Id {  get; set; }
    }
}
