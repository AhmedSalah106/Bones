namespace Bones_App.Exceptions
{
    public class ErrorResponse
    {
        public List<string> Errors { get; set; } = new();
        public int statusCode {  get; set; }
    }
}
